
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using PagedList;
using YoutuberCritics.Data;
using YoutuberCritics.Models;

namespace YoutuberCritics.Services
{
    public class SearchService
    {
        private readonly IEnumerable<RemoteWebDriver> _drivers;
        private readonly YoutuberCriticsContext _context;

        public SearchService(IEnumerable<RemoteWebDriver> drivers, YoutuberCriticsContext context)
        {
            _drivers = drivers;
            _context = context;
        }

        
        private void ShortScroll(RemoteWebDriver driver)
        {
            for (int i = 0; i < 10; i++)
            {
                driver.ExecuteScript("window.scrollBy(0,150)");
                Thread.Sleep(50);
            }
        }

        private void FullScroll(RemoteWebDriver driver)
        {
            while (true){
                long before = (long) driver.ExecuteScript("return window.pageYOffset;");

                this.ShortScroll(driver);
                
                long after = (long) driver.ExecuteScript("return window.pageYOffset;");
                
                if (after - before < 200) break;

                Thread.Sleep(1000);
            }
        }

        private bool IsDriverIdle(RemoteWebDriver driver)
        {
            Console.WriteLine(driver.Url);
            return driver.Url == "data:,";
        }

        private RemoteWebDriver GetIdleDriver()
        {
            foreach (var driver in _drivers)
            {
                if (IsDriverIdle(driver))
                    return driver;   
            }
            return null;
        }

        private int ParseSubsText(string text)
        {
            if (text.Length == 0) return 0;
            if (text == null) return 0;
            
            var nt = text.Split(" ")[0];
            var mult = 1;
            if (nt.EndsWith('K') || nt.EndsWith('k'))
            {
                mult = 1000;
                nt = nt.Substring(0, nt.Length - 1);
            }
            else if (nt.EndsWith('M') || nt.EndsWith('m') || nt.EndsWith(" mill."))
            {
                mult = 1000000;
                nt = nt.Substring(0, nt.Length - 1);
            }
            return (int) (float.Parse(nt, CultureInfo.InvariantCulture) * mult);
        }

        private IEnumerable<Channel> ParseChannels(RemoteWebDriver driver)
        {
            var channels = new List<Channel>();
            var infos = driver.FindElementsByXPath("//*[@id=\"contents\"]/ytd-channel-renderer");

            foreach (var info in infos)
            {
                var imageURL = info.FindElement(By.XPath(".//*[@id=\"img\"]")).GetAttribute("src");
                var title = info.FindElement(By.XPath(".//*[@id=\"text\"]")).Text;
                var fullPath = info.FindElement(By.XPath(".//*[@id=\"main-link\"]")).GetAttribute("href");
                var description = info.FindElement(By.XPath(".//*[@id=\"description\"]")).Text;

                var subsText = info.FindElement(By.XPath(".//*[@id=\"subscribers\"]")).Text;
                var subs = ParseSubsText(info.FindElement(By.XPath(".//*[@id=\"subscribers\"]")).Text);
                
                var path = "";
                var userIndex = fullPath.IndexOf("user");
                var channelIndex = fullPath.IndexOf("channel");
                if (userIndex == -1)
                {
                    path = fullPath.Substring(channelIndex);
                } 
                else
                {
                    path = fullPath.Substring(userIndex);
                }

                if (imageURL != null)
                    channels.Add(new Channel(title, path, imageURL, description, subs));
                /*
                Console.WriteLine(imageURL);
                Console.WriteLine(title);
                Console.WriteLine(path);
                Console.WriteLine(description);
                Console.WriteLine();
                */
            }
            return channels;
        }

        private void UpdateDatabase(IEnumerable<Channel> channels)
        {
            foreach (var channel in channels)
            {
                try
                {
                    _context.Channels.Add(channel);
                    _context.SaveChanges();
                }
                catch (System.Exception)
                {
                    _context.Entry(channel).State = EntityState.Detached;
                    var existing = _context.Channels.Where(c => c.YoutubePath == channel.YoutubePath).First();
                    _context.Entry(existing).State = EntityState.Modified;
                    existing.Title = channel.Title;
                    existing.Description = channel.Description;
                    existing.ImageURL = channel.ImageURL;
                    existing.Subscribers = channel.Subscribers;
                    _context.SaveChanges();
                    Console.WriteLine("DUPLICATE UPDATED!");
                }
            }
        }

        public IEnumerable<Channel> ChannelScrapeSearch(string keyword, bool fullScan) 
        {
            var driver = GetIdleDriver();
            if (driver == null) return new List<Channel>();

            driver.Navigate().GoToUrl("https://www.youtube.com/results?search_query=" + keyword + "&sp=CAMSAhAC");
            
            if (fullScan){
                FullScroll(driver);
            }
            else
            {
                ShortScroll(driver);
            }
            var data = ParseChannels(driver);
            UpdateDatabase(data);
            driver.Navigate().GoToUrl("data:,");
            return data;
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
                    return (channel => channel.Subscribers);
                case 3:
                    return (channel => channel.YoutubePath);
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