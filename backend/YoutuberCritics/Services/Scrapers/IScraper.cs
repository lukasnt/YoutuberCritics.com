
using System.Collections.Generic;
using YoutuberCritics.Models;

namespace YoutuberCritics.Services.Scrapers 
{
    public interface IScraper
    {
        IEnumerable<Channel> ShortScrape(string keyword);
        
        IEnumerable<Channel> FullScrape(string keyword);
    }
}