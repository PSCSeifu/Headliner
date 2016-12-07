using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headliner
{

    public static class UrlSource
    {
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
