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
            UrlSource.UrlSources().ForEach(GetName);
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
       
        public async void HeadlineFilter()
        {
            List<Tuple<Uri, string>> list = UrlSource.UrlSources();
            foreach (var url in list)
            {
                var data = await DownloadHtml(url.Item1).ToObservable();

                PopulateListView(data, url.Item2);
                

                //    this.listView.Items.Add($"From Website {url.Item2} {Environment.NewLine}");

                //    foreach (var text in test.Where(x => x != "").Take(15))
                //    {
                //        this.listView.Items.Add(text.Trim());                                     
                //    }                
                //    this.listView.Items.Add($"----------------------------------{Environment.NewLine}");
                //}
            }

        }

        public void PopulateListView(List<string> dataList, string siteName)
        {
            this.listView.Items.Add($"From Website {siteName} {Environment.NewLine}");

            foreach (var text in dataList.Where(x => x != "").Take(15))
            {
                this.listView.Items.Add(text.Trim());
            }
            this.listView.Items.Add($"----------------------------------{Environment.NewLine}");
        }
    }
}
