
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PagedList;
using YoutuberCritics.Data;
using YoutuberCritics.Models;
using YoutuberCritics.Services.Scrapers;

namespace YoutuberCritics.Services
{
    public class SearchService
    {
        private readonly YoutuberCriticsContext _context;

        private readonly IScraper _scraper;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SearchService(IScraper scraper, YoutuberCriticsContext context, IServiceScopeFactory serviceScopeFactory)
        {
            _scraper = scraper;
            _context = context;
            _serviceScopeFactory = serviceScopeFactory;
        }

        private void UpdateDatabase(IEnumerable<Channel> channels)
        {
            // Needs a new scope because this is gonna run after the http request have been sent back (meaning the request scope have been disposed)
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<YoutuberCriticsContext>();

                Console.WriteLine("ASYNC RUN");
                foreach (var channel in channels)
                {
                    try
                    {
                        db.Channels.Add(channel);
                        db.SaveChanges();
                        Console.WriteLine("ADDED NEW CHANNEL");
                    }
                    catch (System.Exception)
                    {
                        db.Entry(channel).State = EntityState.Detached;
                        var existing = db.Channels.Where(c => c.YoutubePath == channel.YoutubePath).First();
                        db.Entry(existing).State = EntityState.Modified;
                        existing.Title = channel.Title;
                        existing.Description = channel.Description;
                        existing.ImageURL = channel.ImageURL;
                        existing.Subscribers = channel.Subscribers;
                        db.SaveChanges();
                        Console.WriteLine("DUPLICATE UPDATED!");
                    }
                }
                
                Console.WriteLine("The end");
            }
        }

        public IEnumerable<Channel> ExtendChannelData(IEnumerable<Channel> channels)
        {
            var channelPaths = channels.Select(channel => channel.YoutubePath);
            var extendedChannels = _context.Channels.AsNoTracking().Where(channel => channelPaths.Contains(channel.YoutubePath)).ToList();
            var result = new List<Channel>();
            foreach (var channel in channels)
            {
                var extendedChannel = extendedChannels.Find(c => c.YoutubePath == channel.YoutubePath);
                if (extendedChannel != null)
                {
                    result.Add(extendedChannel);
                }
                else{
                    result.Add(channel);
                }
            }
            return result;
        }

        public IEnumerable<Channel> ChannelScrapeSearch(string keyword, bool fullScan) 
        {
            IEnumerable<Channel> channels;
            if (fullScan)
            {
                channels = _scraper.FullScrape(keyword);
            }
            else
            {
                channels = _scraper.ShortScrape(keyword);
            }
            channels = ExtendChannelData(channels);
            Task.Factory.StartNew(() => UpdateDatabase(channels));
            return channels;
        }

        private Expression<Func<Channel, Object>> GetChannelOrderAttribute(int orderEnum)
        {
            switch (orderEnum)
            {
                case 0:
                    return (channel => channel.Subscribers);
                case 1:
                    return (channel => channel.Title);
                case 2:
                    return (channel => channel.RatingAverage);
                case 3:
                    return (channel => channel.ReviewCount);
                case 4:
                    return (channel => channel.ChannelID);
                default:
                    return (review => review.Subscribers);
            }
        }

        public IEnumerable<Channel> ChannelDBSearch(string keyword, int orderEnum, int page, int pageSize)
        {
            int maxItems = 1000;
            var orderExpression = GetChannelOrderAttribute(orderEnum);

            var result = _context.Channels.AsNoTracking();
            if (keyword != "" && keyword != null)
            {
                result = result.Where(channel => channel.Title.Contains(keyword) || channel.Description.Contains(keyword));
            }
            result = result.Take(maxItems)
                .OrderByDescending(orderExpression);
            
            return result.ToPagedList(page, pageSize);
        }

        private Expression<Func<Review, Object>> GetReviewOrderAttribute(int orderEnum)
        {
            switch (orderEnum)
            {
                case 0:
                    return (review => review.DatePosted);
                case 1:
                    return (review => review.Title);
                case 2:
                    return (review => review.Rating);
                case 3:
                    return (review => review.User.Name);
                case 4:
                    return (review => review.Channel.Title);
                case 5:
                    return (review => review.ReviewID);
                default:
                    return (review => review.DatePosted);
            }
        }

        public IEnumerable<Review> ReviewDBSearch(string keyword, int orderEnum, int page, int pageSize)
        {
            var maxItems = 1000;
            var orderExpression = GetReviewOrderAttribute(orderEnum);

            var result = _context.Reviews.AsNoTracking();
            if (keyword != "" && keyword != null)
            {
                result = result
                .Where(review => review.Title.Contains(keyword)     ||
                                 review.Text.Contains(keyword)      || 
                                 review.User.Name.Contains(keyword) || 
                                 review.Channel.Title.Contains(keyword));
            }
            result = result.Take(maxItems)
                .Include(review => review.Channel)
                .Include(review => review.User)
                .OrderByDescending(orderExpression);
            
            return result.ToPagedList(page, pageSize);
        }
    }
}