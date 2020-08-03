
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using YoutuberCritics.Models;

namespace YoutuberCritics.Services.Scrapers 
{
    
    public class DriverScraper : IScraper
    {
        private readonly IEnumerable<RemoteWebDriver> _drivers;

        public DriverScraper(IEnumerable<RemoteWebDriver> drivers)
        {
            _drivers = drivers;
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

        private IEnumerable<Channel> ChannelScrapeSearch(string keyword, bool fullScan) 
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
            driver.Navigate().GoToUrl("data:,");
            return data;
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