using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Telerik.Windows.Controls;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace GitDuck
{
    public partial class RepoPage : PhoneApplicationPage
    {
        private ObservableCollection<RepoData> myRepoItems;
        private ObservableCollection<RepoData> starRepoItems;

        public RepoPage()
        {
            InitializeComponent();
            
        }

        private void myRepoListBox_Loaded(object sender, RoutedEventArgs e)
        {
            myRepoBusyIndicator.IsRunning = true;
            myRepoItems = new ObservableCollection<RepoData>();
            myRepoListBox.ItemsSource = myRepoItems;
            WebClient client = new WebClient();
            client.DownloadStringCompleted += client_myRepoDownloadStringCompleted;
            client.DownloadStringAsync(new System.Uri("https://api.github.com/users/" + (App.Current as App).CurrentUserInfo.login + "/repos"));
        }

        private void client_myRepoDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            List<RepoData> deserialized = JsonConvert.DeserializeObject<List<RepoData>>(e.Result);

            foreach(RepoData repoData in deserialized)
            {
                myRepoItems.Add(repoData);
            }

            myRepoListBox.ItemsSource = myRepoItems;

            if (myRepoItems.Count < 1)
            {
                myRepoListBox.EmptyContent = "You do not have any Repositories!";
            }

            myRepoBusyIndicator.IsRunning = false;
        }

        private void starListBox_Loaded(object sender, RoutedEventArgs e)
        {
            starBusyIndicator.IsRunning = true;
            starRepoItems = new ObservableCollection<RepoData>();
            starListBox.ItemsSource = starRepoItems;
            WebClient client = new WebClient();
            client.DownloadStringCompleted += client_starDownloadStringCompleted;
            client.DownloadStringAsync(new System.Uri("https://api.github.com/users/" + (App.Current as App).CurrentUserInfo.login + "/starred"));
        }

        private void client_starDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            List<RepoData> deserialized = JsonConvert.DeserializeObject<List<RepoData>>(e.Result);

            foreach (RepoData gistData in deserialized)
            {
                starRepoItems.Add(gistData);
            }

            starListBox.ItemsSource = starRepoItems;

            if (starRepoItems.Count < 1)
            {
                starListBox.EmptyContent = "You do not have any Starred Repositories!";
            }

            starBusyIndicator.IsRunning = false;
        }

        private void MainPivot_Loaded(object sender, RoutedEventArgs e)
        {
            if (NavigationContext.QueryString != null && NavigationContext.QueryString.ContainsKey("page"))
            {
                if (NavigationContext.QueryString["page"] == "starred")
                {
                    MainPivot.SelectedItem = starPivotItem;
                }
            }
        }

        private void myRepoListBox_ItemTap(object sender, ListBoxItemTapEventArgs e)
        {
            (App.Current as App).CurrentRepoInfo = (RepoData)e.Item.AssociatedDataItem.Value;
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri("/InfoPages/RepoInfoPage.xaml", UriKind.Relative));
            });
        }
    }
}