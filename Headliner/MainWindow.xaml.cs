using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Headliner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitInterface();
        }

        public void InitInterface()
        {
            HeadlineFilter();
            PopulateSiteListbox();
        }

        public async Task<List<string>> DownloadHtml(Uri url)
        {
            var client = new WebClient();
            var download = client.DownloadString(url);

            var parser = new  AngleSharp.Parser.Html.HtmlParser();
            var result = await parser.ParseAsync(download);

            var headline = result.GetElementsByClassName("title").Select( x => x.TextContent);
            var links = result.GetElementsByTagName("a").Select(x => x.TextContent);
            var logo = result.GetElementsByClassName("logo").Select(x => x.InnerHtml);

            var joined = headline.Union(links.Union(logo));

            return joined.ToList();
        }

        public void PopulateSiteListbox()
        {
            UrlSource().ForEach(GetName);
        }

        public void GetName(Tuple<Uri, string> input)
        {
            this.sitelistBox.Items.Add(input.Item2);
        }

        public async Task<string> GetHtmlAsync( Uri url)
        {
            var client = new WebClient();

            var download = client.DownloadStringTaskAsync(url)
                .ToObservable()
                .Timeout(TimeSpan.FromSeconds(30));
                

           // var result = await download.Subscribe();

            var html = await download;
            return html;
        }
        
        public List<Tuple<Uri, string>> UrlSource()
        {
            List<Tuple<Uri, string>> urlList = new List<Tuple<Uri, string>>();
            urlList.Add(new Tuple<Uri, string>(new Uri("http://www.zerohedge.com"), "ZEROHEDGE"));
            urlList.Add(new Tuple<Uri, string>(new Uri("https://www.codeproject.com"), "CODEPROJECT"));
            urlList.Add(new Tuple<Uri, string>(new Uri("http://www.stackoverflow.com"), "STACKOVERFLOW"));
            urlList.Add(new Tuple<Uri, string>(new Uri("https://fsharpforfunandprofit.com/"), "FSHARPFORFUNANDPROFIT"));
            urlList.Add(new Tuple<Uri, string>(new Uri("http://www.infoq.com"), "INFOQ"));

            return urlList;
        }

        public async void HeadlineFilter()
        {
            foreach (var url in UrlSource())
            {
                var test = await DownloadHtml(url.Item1);

                //this.htmlbox.Text += string.Format("From Website {0}", url.Item2) + Environment.NewLine;
                this.listView.Items.Add(string.Format("From Website {0}", url.Item2) + Environment.NewLine);

                foreach (var text in test.Where(x => x != "").Take(15))
                {
                    this.listView.Items.Add(text.Trim());
                    //this.htmlbox.Text +=   text + Environment.NewLine;                    
                }

               // this.htmlbox.Text += "----------------------------------" + Environment.NewLine;
                this.listView.Items.Add("----------------------------------" + Environment.NewLine);                
            }

        }

       


    }
}
