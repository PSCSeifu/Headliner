using AngleSharp.Dom.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headliner.Lib
{
    public class SearchEngine
    {
        public static List<string> SearchByWord(IHtmlDocument doc, string searchWord)
        {
            var joined = doc.GetElementsByClassName("title").Select(x => x.TextContent)
                .Union(doc.GetElementsByTagName("a href").Select(x => x.TextContent))
                .Union(doc.GetElementsByClassName("main-promobox__link").Select(x => x.TextContent)
                ).ToList();
            List<string> filterd = new List<string>();

            foreach (var item in joined)
            {
                filterd.Add(item);
            }
            return filterd;
        }
    }
}
