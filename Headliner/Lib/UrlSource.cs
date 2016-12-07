using Headliner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headliner
{

    public static class UrlSource
    {

        public static List<Website> GetWebsites()
        {
            List<Website> websites = new List<Website>();
            websites.Add( new  Website(new Uri("http://www.zerohedge.com"), "ZEROHEDGE"));
            websites.Add(new Website(new Uri("https://www.codeproject.com"), "CODEPROJECT"));
            websites.Add(new Website(new Uri("https://fsharpforfunandprofit.com/"), "FSHARPFORFUNANDPROFIT"));
            websites.Add(new Website(new Uri("http://www.infoq.com"), "INFOQ"));
            websites.Add(new Website(new Uri("http://www.rt.com"), "RT"));

            return websites;
        }

        public static List<Tuple<Uri, string>> UrlSources()
        {
            List<Tuple<Uri, string>> urlList = new List<Tuple<Uri, string>>();
            urlList.Add(new Tuple<Uri, string>(new Uri("http://www.zerohedge.com"), "ZEROHEDGE"));
            urlList.Add(new Tuple<Uri, string>(new Uri("https://www.codeproject.com"), "CODEPROJECT"));            
            urlList.Add(new Tuple<Uri, string>(new Uri("https://fsharpforfunandprofit.com/"), "FSHARPFORFUNANDPROFIT"));
            urlList.Add(new Tuple<Uri, string>(new Uri("http://www.infoq.com"), "INFOQ"));
            urlList.Add(new Tuple<Uri, string>(new Uri("http://www.rt.com"), "RT"));

            return urlList;
        }
    }
}
