using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headliner.Lib
{
    public class Tools
    {
        public static string _jsonPath = System.Configuration.ConfigurationManager.AppSettings["WebSiteFilePath"];
        public static string _waitingGifPath = System.Configuration.ConfigurationManager.AppSettings["WaitingGifFilePath"];
        public static string _debugtraceLevel = System.Configuration.ConfigurationManager.AppSettings["DebugTraceLevel"];

        public static  void DebugTrace(string processName, int threadId)
        {
            if(Convert.ToBoolean(_debugtraceLevel))
            {
                Debug.WriteLine($"Current thread Id in {processName}: {threadId.ToString()}");
            }
        }
    }
}
