using Microsoft.AspNetCore.Mvc;
using YoutuberCritics.Services;

namespace YoutuberCritics
{

    [Route("api/search")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly SearchService _search;
        
        public SearchController(SearchService search)
        {
            _search = search;
        }


        [HttpGet]
        public ActionResult SearchChannels([FromQuery] string keyword, [FromQuery] bool fullScan, [FromQuery] bool scrape)
        {
            if (scrape)
            {
                return Ok(_search.ChannelScrapeSearch(keyword, fullScan));
            }
            else 
            {
                return Ok(_search.ChannelDBSearch(keyword, fullScan));
            }
        }
    }
}