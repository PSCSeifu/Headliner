using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headliner.Models
{
    public class Website
    {
        public Uri WebSiteUri { get; set; }
        public string SiteName { get; set; }
        [JsonIgnore]
        [JsonProperty(Required = Required.Default)]
        public bool Status { get; set; }

        public Website()
        {
                
        }

        public Website(Uri uri, string name)
        {
            WebSiteUri = uri;
            SiteName = name;
        }

        public Website(Uri uri, string name, bool status)
        {
            WebSiteUri = uri;
            SiteName = name;
            Status = status;
        }
    }
}
