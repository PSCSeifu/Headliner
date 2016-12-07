using Headliner.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headliner.Lib
{
    public class TheInternet
    {
        private static string _jsonPath = System.Configuration.ConfigurationManager.AppSettings["WebSiteFilePath"];

        public static List<Website> ReadFile()
        {
            List<Website> sites = new List<Website>();
            try
            {
                using (StreamReader sr = new StreamReader(_jsonPath))
                {
                    string json = sr.ReadToEnd();
                    sites = JsonConvert.DeserializeObject<List<Website>>(json);
                }
            }
            catch { sites = GetWebsites(); }

            return sites;
        }

        private static List<Website> GetWebsites()
        {
            List<Website> websites = new List<Website>();
            websites.Add(new Website(new Uri("http://www.zerohedge.com"), "ZEROHEDGE",false));
            websites.Add(new Website(new Uri("https://www.codeproject.com"), "CODEPROJECT", false));
            websites.Add(new Website(new Uri("https://fsharpforfunandprofit.com/"), "FSHARPFORFUNANDPROFIT", false));
            websites.Add(new Website(new Uri("http://www.infoq.com"), "INFOQ", false));
            websites.Add(new Website(new Uri("http://www.rt.com"), "RT", false));

            return websites;
        }

    }
}
