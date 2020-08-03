
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using YoutuberCritics.Models;

namespace YoutuberCritics.Services.Scrapers 
{

    public class HttpScraper : IScraper
    {
        private int ParseSubsText(JToken textToken)
        {
            if (textToken == null) return 0;
            var text = textToken.ToString();
            if (text.Length == 0) return 0;
            
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

        private string ParseDescription(JToken textList)
        {
            if (textList == null) return "";
            string description = "";
            foreach (var textItem in textList)
            {
                description += textItem.SelectToken("text") == null ? "" : textItem.SelectToken("text").ToString();
            }
            return description;
        }

        private string ParseImageURL(JToken urlToken)
        {
            if (urlToken == null) return "";
            var url = urlToken.ToString();
            if (!url.StartsWith("http")) return "https:" + url;
            return url;
        }

        public List<Channel> ParseChannelList(JToken channelList)
        {
            var channels = new List<Channel>();
            foreach (var channel in channelList)
            {
                var ch = channel.SelectToken("channelRenderer");
                if (ch == null) continue;
                var title = ch.SelectToken("title.simpleText") == null ? "" : ch.SelectToken("title.simpleText").ToString();
                var channelID = ch.SelectToken("channelId").ToString();
                var path = ch.SelectToken("navigationEndpoint.browseEndpoint.canonicalBaseUrl").ToString().Substring(1);
                var description = ParseDescription(ch.SelectToken("descriptionSnippet.runs"));
                var imageURL = ParseImageURL(ch.SelectToken("thumbnail.thumbnails[1].url"));
                var subs = ParseSubsText(ch.SelectToken("subscriberCountText.simpleText"));

                channels.Add(new Channel(title, path, imageURL, description, subs));

                /*
                Console.WriteLine(title);
                Console.WriteLine(channelID);
                Console.WriteLine(path);
                Console.WriteLine(imageURL);
                Console.WriteLine(subs);
                Console.WriteLine("----------------");
                */
            }
            return channels;
        }

        private static readonly WebClient client = new WebClient();

        private IEnumerable<Channel> ChannelScrapeSearch(string keyword, bool fullScan)
        {
            Console.WriteLine(keyword);
            if (keyword == null || keyword == "") return new List<Channel>();
            client.Headers.Add("Accept-Language", "en-US");
            var url = "https://www.youtube.com/results?search_query=" + keyword + "&sp=CAMSAhAC";
            var doc = new HtmlDocument();
            doc.Load(client.OpenRead(url), System.Text.Encoding.UTF8, true);
            
            var script = doc.DocumentNode.SelectSingleNode("/html/body/script[2]").InnerText;
            int start = script.IndexOf("=") + 1;
            script = script.Substring(start, script.LastIndexOf("}}};") + 3 - start);
            var initialData = JObject.Parse(script);

            var channelList = initialData.SelectToken("contents.twoColumnSearchResultsRenderer.primaryContents.sectionListRenderer.contents[0].itemSectionRenderer.contents");
            var channels = ParseChannelList(channelList);

            if (channels.Count == 0)
            {
                channelList = initialData.SelectToken("contents.twoColumnSearchResultsRenderer.primaryContents.sectionListRenderer.contents[1].itemSectionRenderer.contents");
                channels = ParseChannelList(channelList);
            }

            if (fullScan)
            {
                var continuationToken = initialData.SelectToken("contents.twoColumnSearchResultsRenderer.primaryContents.sectionListRenderer.contents[1].continuationItemRenderer.continuationEndpoint.continuationCommand.token").ToString();
                channels.AddRange(ContinuationScrape(continuationToken));
            }

            return channels;
        }
        
        
        public IEnumerable<Channel> ContinuationScrape(string initContToken)
        {
            var continuationToken = initContToken;
            var channels = new List<Channel>();
            while (continuationToken != null)
            {
                //Console.WriteLine(continuationToken);
                
                var continuationPost = @"{
                    'context': {
                        'client': {
                            'clientName': 'WEB',
                            'clientVersion': '2.20200728.06.00'
                        }
                    },
                    'continuation': '" + continuationToken + @"'
                }";
                
                //Console.WriteLine(continuationPost);

                var response = client.UploadString("https://www.youtube.com/youtubei/v1/search?key=AIzaSyAO_FJ2SlqU8Q4STEHLGCilw_Y9_11qcW8", continuationPost);
                var obj = JObject.Parse(response);
                channels.AddRange(ParseChannelList(obj.SelectToken("onResponseReceivedCommands[0].appendContinuationItemsAction.continuationItems[0].itemSectionRenderer.contents")));
                var continuationObj = obj.SelectToken("onResponseReceivedCommands[0].appendContinuationItemsAction.continuationItems[1].continuationItemRenderer.continuationEndpoint.continuationCommand.token");
                continuationToken = continuationObj == null ? null : continuationObj.ToString();
            }
            return channels;
        }
        
        public IEnumerable<Channel> ShortScrape(string keyword)
        {
            return ChannelScrapeSearch(keyword, false);
        }

        public IEnumerable<Channel> FullScrape(string keyword)
        {
            return ChannelScrapeSearch(keyword, true);
        }
    }

}