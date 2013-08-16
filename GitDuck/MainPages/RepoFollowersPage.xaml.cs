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
    public partial class RepoFollowersPage : PhoneApplicationPage
    {
        private ObservableCollection<BasicUser> followerItems;

        public RepoFollowersPage()
        {
            InitializeComponent();
        }

        private void followersListBox_Loaded(object sender, RoutedEventArgs e)
        {
            followersBusyIndicator.IsRunning = true;
            followerItems = new ObservableCollection<BasicUser>();
            followersListBox.ItemsSource = followerItems;
            WebClient client = new WebClient();
            client.DownloadStringCompleted += client_followersDownloadStringCompleted;
            client.DownloadStringAsync(new System.Uri("https://api.github.com/repos/" + (App.Current as App).CurrentRepoInfo.full_name + "/subscribers"));
        }

        void client_followersDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            List<BasicUser> deserialized = JsonConvert.DeserializeObject<List<BasicUser>>(e.Result);

            foreach (BasicUser followerData in deserialized)
            {
                followerItems.Add(followerData);
            }

            followersListBox.ItemsSource = followerItems;

            if (followerItems.Count < 1)
            {
                followersListBox.EmptyContent = "This Repo does not have any Followers!";
            }

            followersBusyIndicator.IsRunning = false;
        }

    }
}