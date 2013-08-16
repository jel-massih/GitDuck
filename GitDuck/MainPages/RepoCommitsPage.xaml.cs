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
    public partial class RepoCommitsPage : PhoneApplicationPage
    {
        private ObservableCollection<CommitData> commitItems;

        public RepoCommitsPage()
        {
            InitializeComponent();
        }

        private void commitsListBox_Loaded(object sender, RoutedEventArgs e)
        {
            commitsBusyIndicator.IsRunning = true;
            commitItems = new ObservableCollection<CommitData>();
            commitsListBox.ItemsSource = commitItems;
            WebClient client = new WebClient();
            client.DownloadStringCompleted += client_commitsDownloadStringCompleted;
            client.DownloadStringAsync(new System.Uri("https://api.github.com/repos/" + (App.Current as App).CurrentRepoInfo.full_name + "/commits"));
        }

        void client_commitsDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            List<CommitData> deserialized = JsonConvert.DeserializeObject<List<CommitData>>(e.Result);

            foreach (CommitData commitData in deserialized)
            {
                commitData.commitDetail = "by " + commitData.commit.author.name;

                DateTime publishDate = HelperMethods.GitHubDateToDateTime(commitData.commit.author.date);
                commitData.publishDate = HelperMethods.GetMonth(publishDate.Month) + " " + publishDate.Day + " at " + publishDate.Hour + ":" + publishDate.Minute;

                commitItems.Add(commitData);
            }

            commitsListBox.ItemsSource = commitItems;

            if (commitItems.Count < 1)
            {
                commitsListBox.EmptyContent = "You do not have any Commits!";
            }

            commitsBusyIndicator.IsRunning = false;
        }
    }
}