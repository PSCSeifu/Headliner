using AngleSharp.Dom.Html;
using Headliner.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
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

            Tools.DebugTrace(" Lib.GetHtmlByWebsite", Thread.CurrentThread.ManagedThreadId);
            
            return result;
        }

        public static List<string> GetHeadlineList(IHtmlDocument doc,string tagname)
        {
            return  doc.GetElementsByClassName(tagname).Select(x => x.TextContent).ToList();                
        }

        public static async Task<List<string>> DownloadHtml(Website website)
        {
            Tools.DebugTrace("Lib.DownloadHtml", Thread.CurrentThread.ManagedThreadId);
            var result = await Downloader.GetHtmlByWebsite(website);
            return Downloader.GetHeadlineList(result,website.ClassTag).ToList();
        }
    }
}
