using AngleSharp.Dom.Html;
using Headliner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headliner.Lib
{
    public class SearchEngine
    {
        public static async Task<List<string>> SearchByWord(Website website, string searchWord)
        {
            var result = await Downloader.DownloadHtml(website);

            var filtered = result.Where(x => x.Contains(searchWord))
                .Select(x => x)
                .ToList();                
            
            return filtered;
        }


        public static async Task<List<string>> Search(Website website,string word)
        {
            return await SearchByWord(website, word);
        }

        public static async Task<List<string>> SearchWebSites(List<Website> websites, string searchWord)
        {
            List<string> resultList = new List<string>();
            foreach (var site in websites)
            {
                resultList = await  SearchByWord(site, searchWord);
            }

            return resultList;
           
        }
    }
}
