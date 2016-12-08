using Headliner.Lib;
using Headliner.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
            //Trace.WriteLine($"Thread Name : {SynchronizationContext.Current.ToString()}");
            InitializeComponent();

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
           // ShowWaitingGif(this.spinner);
            HeadlineFilter();
            PopulateSiteListbox();
        }

        public  async void HeadlineFilter()
        {
            //this.listView.Items.Clear();
            foreach (var site in TheInternet.ReadFile())
            {
                var data = await Downloader.DownloadHtml(site);
                var list = data.ToObservable()                                
                                .Take(5)                               
                                .Select(x => x);

                list.Subscribe(
                     x => PopulateView(x, site.SiteName)
                    );          
            }
        }

        #region UI Methods

        public void ShowWaitingGif(Image element )
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(TheInternet._waitingGifPath);
            image.EndInit();
            ImageBehavior.SetAnimatedSource(element, image);
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
