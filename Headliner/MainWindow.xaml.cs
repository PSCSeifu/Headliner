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

namespace Headliner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {      
        public MainWindow()
        {
            Debug.WriteLine($"Current thread Id in ctor : {Thread.CurrentThread.ManagedThreadId}");
            InitializeComponent();
            ShowWaitingGif(this.spinner, true);

            var buttonClick = Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>
                (
                p => this.button.Click += p,
                p => this.button.Click -= p);

            buttonClick.Subscribe(Window =>
           {
               InitInterface();
           });
            
        }

        public void InitInterface()
        {
            TestStream();
           // HeadlineFilter2();
            //ShowHeadlines();
            // HeadlineFilter();
            //PopulateSiteListbox();
            //ShowWaitingGif(this.spinner, false);
        }

        public async void TestStream()
        {
            Debug.WriteLine($"Current thread Id a : {Thread.CurrentThread.ManagedThreadId}");
            //var seq = Observable.Interval(TimeSpan.FromMilliseconds(200));
            var res = await Task.Run( () => TheInternet.TestLongProcess());

            var seq = res.ToObservable();




            Debug.WriteLine($"Current thread Id b : {Thread.CurrentThread.ManagedThreadId}");
            var ui = seq
                        .ObserveOn(Dispatcher)
                         .Select(x => x)
                        .Take(90)
                        .Subscribe(test => this.listView.Items.Add(test));
        }

        public async void HeadlineFilter2()
        {
            if (this.listView.Items.Count > 0) { this.listView.Items.Clear(); }

            Debug.WriteLine($"Current thread Id before loop : {Thread.CurrentThread.ManagedThreadId}");

            foreach (var site in TheInternet.ReadFile())
            {
                Debug.WriteLine($"Current thread Id in loop: {Thread.CurrentThread.ManagedThreadId}");
                var data = await Downloader.DownloadHtml(site);

                //var dataStream = data.ToObservable()
                //                .ObserveOn(SynchronizationContext.Current)
                //                .Where( x => x.Length > 5)
                //                .Select( x => x.Trim())
                //                .Take(9)                            
                //                .Subscribe(test => this.listView.Items.Add($"Site : {site.SiteName} | {Environment.NewLine} {test}"));

                var ui = data.ToObservable()
                                .ObserveOn(Dispatcher)
                                .Where(x => x.Length > 5)
                                .Select(x => x.Trim())
                                .Take(9)
                                .Subscribe(test => this.listView.Items.Add($"Site : {site.SiteName} | {Environment.NewLine} {test}"));
                Debug.WriteLine($"Current thread Id in loop: {Thread.CurrentThread.ManagedThreadId}");
            }
        }

        public void ShowHeadlines()
        {
            //TheInternet.ReadFile().ForEach(HeadlineFilter2);
        }

        public async void HeadlineFilter()
        {
            if (this.listView.Items.Count > 0) { this.listView.Items.Clear(); }

            //var seq = Observable.Interval(TimeSpan.FromMilliseconds(1500))
            //    .Select( x => x)
            //    .ObserveOn(SynchronizationContext.Current)
            //    .Subscribe( test => this.listView.Items.Add(test));

            foreach (var site in TheInternet.ReadFile())
            {
                var data = await Downloader.DownloadHtml(site);
                var list = data.ToObservable()
                    .SubscribeOn(NewThreadScheduler.Default)
                                .Take(5)
                                .Select(x => x);

                list.ObserveOn(SynchronizationContext.Current).Subscribe(
                     x => PopulateView(x, site.SiteName)
                    );
            }
            // return 1;
        }

        #region UI Methods

        public void ShowWaitingGif(Image element,bool show )
        {
            var image = new BitmapImage();
            if (show)
            {               
                image.BeginInit();
                image.UriSource = new Uri(TheInternet._waitingGifPath);
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

        public void PopulateView(string text, string siteName)
        {
            
            this.listView.Items.Add($"Site : {siteName} | {Environment.NewLine} {text.Trim()}");          
        }
      
        public void PopulateSiteListbox()
        {
            this.sitelistBox.Items.Clear();
            TheInternet.ReadFile().ForEach(GetName);
        }

        public void GetName(Website site)
        {
            this.sitelistBox.Items.Add(site.SiteName);
        }

        #endregion
    }
}
