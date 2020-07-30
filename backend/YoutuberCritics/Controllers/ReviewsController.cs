
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YoutuberCritics.Data;
using YoutuberCritics.Models;
using YoutuberCritics.Services;

namespace YoutuberCritics.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewsController : ControllerBase 
    {
        private readonly YoutuberCriticsContext _context;
        private readonly CacheService _cache;
        public ReviewsController(YoutuberCriticsContext context, CacheService cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpGet("recent/")]
        public ActionResult<IEnumerable<Review>> GetRecentReviews() 
        {
            var maxItems = 8;
            if (_cache.GetReviews() == null || _cache.GetReviews().Count() < maxItems)
            {
                var result = _context.Reviews
                    .OrderByDescending(review => review.DatePosted)
                    .Take(maxItems)
                    .Include(review => review.Channel)
                    .Include(review => review.User)
                    .ToList();
                _cache.SetReviews(result);
                return Ok(result);
            }
            else
            {
                return Ok(_cache.GetReviews().Take(maxItems));
            }
            
        }

        [HttpGet()]
        public ActionResult<IEnumerable<Review>> GetChannelReviews([FromQuery] string pathname) 
        {
            var channel = _context.Channels.First(channel => channel.YoutubePath == pathname);
            if (channel == null) 
                return NotFound();
            else
                return Ok(_context.Reviews
                    .Include(review => review.User)
                    .Where(review => review.ChannelID == channel.ChannelID)
                    .OrderByDescending(review => review.DatePosted));
        }
    }
}