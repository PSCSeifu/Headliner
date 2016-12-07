using Headliner.Lib;
using Headliner.Models;
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

        public async Task<List<string>> DownloadHtml(Website website)
        {
            var result = await Downloader.GetHtmlByWebsite(website);
            return Downloader.GetHeadlineList(result).ToList();

            //var headline = result.GetElementsByClassName("title").Select( x => x.TextContent);
            //var links = result.GetElementsByTagName("a").Select(x => x.TextContent);
            //var logo = result.GetElementsByClassName("logo").Select(x => x.InnerHtml);

            //var joined = headline.Union(links.Union(logo));

            //return joined.ToList();
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
            foreach (var site in TheInternet.ReadFile())
            {
                var data = await DownloadHtml(site);
                var list = data.ToObservable().Take(5).Select(x =>x);

                list.Subscribe(
                     x => PopulateView(x, site.SiteName)
                    );

                //PopulateListView(data.Latest().Single(), site.SiteName);                
            }

        }
        #region UI Methods

        public void PopulateView(string text, string siteName)
        {
            //this.listView.Items.Add($"From Website {siteName} {Environment.NewLine}");

            //foreach (var text in dataList.Where(x => x != "").Take(15))
            //{
                this.listView.Items.Add($"Site : {siteName} | {Environment.NewLine} {text.Trim()}");
            //}
           // this.listView.Items.Add($"-----------------------------------------------------------{Environment.NewLine}");
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

        public void PopulateSiteListbox()
        {
            TheInternet.ReadFile().ForEach(GetName);
        }

        public void GetName(Website site)
        {
            this.sitelistBox.Items.Add(site.SiteName);
        }

        #endregion
    }
}
