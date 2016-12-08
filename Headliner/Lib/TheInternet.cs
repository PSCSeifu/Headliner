using Headliner.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Headliner.Lib
{
    public class TheInternet
    {
        private static string _jsonPath = System.Configuration.ConfigurationManager.AppSettings["WebSiteFilePath"];
        public static string _waitingGifPath = System.Configuration.ConfigurationManager.AppSettings["WaitingGifFilePath"];

        public static  List<Website> ReadFile()
        {
            List<Website> sites = new List<Website>();
            try
            {
                using (StreamReader sr = new StreamReader(_jsonPath))
                {
                    string json =  sr.ReadToEnd();
                    sites =  JsonConvert.DeserializeObject<List<Website>>(json);
                }
            }
            catch(Exception ex)
            {
                var x = ex;
                sites = GetWebsites();
            }

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

            return  websites;
        }


        public  static  async  Task<List<int>> TestLongProcess(int x)
        {
            Debug.WriteLine($"Current thread Id in Lib : {Thread.CurrentThread.ManagedThreadId}");
            List<int> collec = new List<int>();
            for (int i = x; i > 0; i--)
            {
                var bl = await fakeProcess();
                 Thread.Sleep(100);
                 collec.Add(i);
            }
            return  collec;
        }
       
        private static  async Task<string>  fakeProcess()
        {
            Task<string> aaa = Task.Run(
                () => {
                    Thread.Sleep(10);
                    return "";
                });
            string str = await aaa;
            return str;
        }

    }
}
