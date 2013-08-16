using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using GitDuck.HelperClasses;
using Newtonsoft.Json;

namespace GitDuck.MainPages
{
    public partial class RepoStargazerPage : PhoneApplicationPage
    {
        private ObservableCollection<BasicUser> stargazerItems;

        public RepoStargazerPage()
        {
            InitializeComponent();
        }

        private void stargazersListBox_Loaded(object sender, RoutedEventArgs e)
        {
            stargazersBusyIndicator.IsRunning = true;
            stargazerItems = new ObservableCollection<BasicUser>();
            stargazersListBox.ItemsSource = stargazerItems;
            WebClient client = new WebClient();
            client.DownloadStringCompleted += client_stargazersDownloadStringCompleted;
            client.DownloadStringAsync(new System.Uri("https://api.github.com/repos/" + (App.Current as App).CurrentRepoInfo.full_name + "/stargazers"));
        }

        void client_stargazersDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            List<BasicUser> deserialized = JsonConvert.DeserializeObject<List<BasicUser>>(e.Result);

            foreach (BasicUser stargazerData in deserialized)
            {
                stargazerItems.Add(stargazerData);
            }

            stargazersListBox.ItemsSource = stargazerItems;

            if (stargazerItems.Count < 1)
            {
                stargazersListBox.EmptyContent = "This Repo does not have any Stargazers!";
            }

            stargazersBusyIndicator.IsRunning = false;
        }

    }
}