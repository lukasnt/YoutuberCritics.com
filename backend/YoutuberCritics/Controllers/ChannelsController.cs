using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YoutuberCritics.Data;
using YoutuberCritics.Models;
using YoutuberCritics.Services;

namespace YoutuberCritics.Controllers
{
    [Route("api/channels")]
    [ApiController]
    public class ChannelsController : ControllerBase
    {
        private readonly YoutuberCriticsContext _context;
        private readonly CacheService _cache;

        public ChannelsController(YoutuberCriticsContext context, CacheService cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Channel>> GetAllChannels([FromQuery] int maxItems = 8)
        {
            if (_cache.GetChannels() == null || _cache.GetChannels().Count() < maxItems)
            {
                var result = _context.Channels.OrderByDescending(channel => channel.Subscribers).Take(maxItems).ToList();
                _cache.SetChannels(result);
                return Ok(result);
            }
            else
            {
                return Ok(_cache.GetChannels());
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Channel> GetChannel(int id)
        {
            var channel = _context.Channels.Find(id);
            if (channel == null) 
                return NotFound();
            else
                return Ok(channel);
        }

        [HttpGet("user/")]
        public ActionResult<Channel> GetChannel([FromQuery] string pathName)
        {
            Console.WriteLine(pathName);
            var channel = _context.Channels.First(channel => channel.YoutubePath == pathName);
            if (channel == null) 
                return NotFound();
            else
                return Ok(channel);
        }

        [HttpGet("{id}/reviews")]
        public ActionResult<IEnumerable<Review>> GetChannelReviews(int id)
        {
            if (_context.Channels.Find(id) == null) 
                return NotFound();

            var reviews = _context.Reviews.Include(r => r.Channel).Include(r => r.User).Where(r => r.Channel.ChannelID == id);

            return Ok(reviews);
        }

        [HttpPost]
        public ActionResult AddChannel([FromBody] Channel channel)
        {
            _context.Channels.Add(channel);
            _context.SaveChanges();
            return CreatedAtAction("AddChannel", new { Id = channel.ChannelID }, channel);
        }

        [HttpPost("{id}/reviews")]
        public ActionResult AddChannelReview(int id, [FromBody] Review review)
        {
            _context.Reviews.Add(review);
            _context.SaveChanges();
            _cache.AddReview(_context.Reviews.Include(review => review.Channel).Include(review => review.User).First(r => r.ReviewID == review.ReviewID));
            return CreatedAtAction("AddChannelReview", new {Id = review.ReviewID}, review);
        }
    }
}