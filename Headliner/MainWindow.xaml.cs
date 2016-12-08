using Headliner.Lib;
using Headliner.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading;
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
using WpfAnimatedGif;
using static Headliner.Models.Enums;

namespace Headliner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {      
        public MainWindow()
        {
           Tools.DebugTrace("ctor", Thread.CurrentThread.ManagedThreadId);            
            InitializeComponent();
            ShowWaitingGif(this.spinner, true);
            
            var buttonClick = Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>
                (
                p => this.Getall.Click += p,
                p => this.Getall.Click -= p);

            buttonClick.Subscribe(Window =>
           {
               InitInterface();
           });

                    }

        public void InitInterface()
        {
            //TestStream();
            DisplayBulkHeadLines(WebsiteType.None,6);            
            ShowWaitingGif(this.spinner, false);
        }

        public async void TestStream()
        {
            for (int i = 100; i > 0; i--)
            {
                var res = await Task.Run(() => TheInternet.TestLongProcess(i));

                var seq = res.ToObservable();
                Debug.WriteLine($"Current thread Id b : {Thread.CurrentThread.ManagedThreadId}");
                var ui = seq
                            .ObserveOn(Dispatcher)
                             .Select(x => x)
                            .Take(90)
                            .Subscribe(test => this.listView.Items.Add(test));
            }
        }

        //public async void Search(WebsiteType type, string text, int headlines)
        //{
        //    int countSites = 0;
        //    List<Website> allSites = TheInternet.ReadFile(type);
        //    int totalSites = (allSites.Count() == 0) ? 1 : allSites.Count();

        //    if (this.listView.Items.Count > 0) { this.listView.Items.Clear(); }
        //    if (this.sitelistBox.Items.Count > 0) { this.sitelistBox.Items.Clear(); }

        //    PopulateSiteListbox();

        //    foreach (var site in allSites)
        //    {
        //        countSites++;
        //        SearchSite(site, text, headlines);
        //    }
        //}
        private IDisposable statusLabel;

        public async void DisplayBulkHeadLines(WebsiteType type, int headlines)
        {
            int countSites = 0;            
            List<Website> allSites = TheInternet.ReadFile(type);
            int totalSites = (allSites.Count() == 0 ) ? 1:allSites.Count();

            if (this.listView.Items.Count > 0) { this.listView.Items.Clear(); }
            if (this.sitelistBox.Items.Count > 0) { this.sitelistBox.Items.Clear(); }

            PopulateSiteListbox();
            


            foreach (var site in allSites)
            {
                Tools.DebugTrace("siteloop", Thread.CurrentThread.ManagedThreadId);
                
                countSites++;
                var data = await Task.Run( () => Downloader.DownloadHtml(site));

                var listView = data.ToObservable()
                                .ObserveOn(Dispatcher)
                                .Where(x => x.Length > 5)
                                .Select(x => x.Trim())
                                .Take(headlines)
                                .Subscribe(x => PopulateView(site, x));

                statusLabel = data.ToObservable()
                    .ObserveOn(Dispatcher)
                    .Subscribe(x => PopulateLabel($"From {site.SiteName}  |  Feteched {countSites} of {totalSites}"));
            }
            statusLabel.Dispose();
        }

        public async void SearchSite (Website website, string text, int headlines)
        {
            //Convert the TextCHanged event to IObservable<string>
            var searchInput = Observable.FromEventPattern(this.textBox, "TextChanged")
                        .Select(e => this.textBox.Text)
                        .Throttle(TimeSpan.FromSeconds(0.5))
                        .DistinctUntilChanged();

            var res = await Task.Run(() =>  SearchEngine.Search(website, searchInput.ToString()));

            res.ToObservable()
                .Select( x => x)
                .Subscribe(y => PopulateView(website,y));
        }

        #region UI Methods


        public void PopulateLabel(string value)
        {
            this.statuslabel.Content = value;
        }

        public void ShowWaitingGif(Image element,bool show )
        {
            var image = new BitmapImage();
            if (show)
            {               
                image.BeginInit();
                image.UriSource = new Uri(Tools._waitingGifPath);
                image.EndInit();
                ImageBehavior.SetAnimatedSource(element, image);
            }
            else
            {
                image.UriSource = null;
                //var controller = ImageBehavior.GetAnimationController(element);
                //controller.Pause();
            }

        }

        public void PopulateView(Website site, string text)
        {            
            this.listView.Items.Add($"Site : {site.SiteName} | {Environment.NewLine} {text.Trim()}");          
        }
      
        public void PopulateSiteListbox()
        {
            TheInternet.ReadFile().ForEach( site => this.sitelistBox.Items.Add(site.SiteName));
        }

        #endregion

        private async void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Website> allSites = TheInternet.ReadFile();

            var searchInput = Observable.FromEventPattern(this.textBox, "TextChanged")
                        .Select(w => this.textBox.Text)
                        .Throttle(TimeSpan.FromSeconds(0.5))
                        .DistinctUntilChanged();
                                    

            Tools.DebugTrace("ctor", Thread.CurrentThread.ManagedThreadId);
            Tools.DebugTrace(searchInput.ToString(), Thread.CurrentThread.ManagedThreadId);

            //var res = await Task.Run( () => SearchEngine.SearchWebSites(allSites, searchInput.ToString()) );
            foreach (var site in allSites)
            {
                var res = await Task.Run(() => SearchEngine.Search(site, searchInput.ToString()));

                res.ToObservable()
                    .Select(x => x)
                    .Subscribe(y => PopulateView(site, y));
            }
           
        }
    }
}
