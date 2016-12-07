using AngleSharp.Dom.Html;
using Headliner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headliner.Lib
{
    public class Downloader
    {
        public static async Task<IHtmlDocument> GetHtmlByWebsite(Website website)
        {
            var client = new WebClient();
            var download =  client.DownloadString(website.WebSiteUri);

            var parser = new AngleSharp.Parser.Html.HtmlParser();
            var result = await parser.ParseAsync(download);

            return result;
        }

        //main-promobox__link
        public static  List<string> GetHeadlineList(IHtmlDocument doc)
        {
            var joined = doc.GetElementsByClassName("title").Select(x => x.TextContent)
                .Union(doc.GetElementsByTagName("a href").Select(x => x.TextContent))
                .Union(doc.GetElementsByClassName("main-promobox__link").Select(x => x.TextContent)
                ).ToList();


            return  joined;
        }

    }
}
