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
        public ActionResult SearchChannels([FromQuery] string keyword, [FromQuery] bool fullScan = false, [FromQuery] bool scrape = false, [FromQuery] int order = 0, [FromQuery] int page = 1, [FromQuery] int pageSize = 15)
        {
            if (scrape)
            {
                return Ok(_search.ChannelScrapeSearch(keyword, fullScan));
            }
            else 
            {
                return Ok(_search.ChannelDBSearch(keyword, order, page, pageSize));
            }
        }

        [HttpGet("reviews")]
        public ActionResult SearchReviews([FromQuery] string keyword, [FromQuery] int order = 0, [FromQuery] int page = 1, [FromQuery] int pageSize = 15)
        {
            return Ok(_search.ReviewDBSearch(keyword, order, page, pageSize));
        }
    }
}