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
using System.Collections.ObjectModel;

namespace GitDuck
{
    public partial class RepoInfoPage : PhoneApplicationPage
    {
        private RepoData MyRepoData;
        private ObservableCollection<RepoHubListItem> HubList;

        public RepoInfoPage()
        {
            InitializeComponent();

            MyRepoData = (App.Current as App).CurrentRepoInfo;
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            repoTitleTF.Text = "GitHub |  " + MyRepoData.full_name;
        }

        private void repoTileList_Loaded(object sender, RoutedEventArgs e)
        {
            HubList = new ObservableCollection<RepoHubListItem>();
            repoTileList.ItemsSource = HubList;
            if (MyRepoData.open_issues_count > 0)
            {
                HubList.Add(new RepoHubListItem("Issues", "/Images/issue-icon.png"));
            }
            HubList.Add(new RepoHubListItem("Commits", "/Images/commit-icon.png"));
            HubList.Add(new RepoHubListItem("Source", "/Images/code-icon.png"));
            if (MyRepoData.has_wiki)
            {
                HubList.Add(new RepoHubListItem("Wiki", "/Images/book-icon.png"));
            }
            HubList.Add(new RepoHubListItem("Followers", "/Images/follower-icon.png"));
            HubList.Add(new RepoHubListItem("Stargazers", "/Images/star-icon.png"));
        }

        private void repoTileList_ItemTap(object sender, Telerik.Windows.Controls.ListBoxItemTapEventArgs e)
        {
            switch (((RepoHubListItem)e.Item.AssociatedDataItem.Value).itemName)
            {
                case "Issues":
                    OpenRepoIssuePage();
                    break;
                case "Commits":
                    OpenRepoCommitPage();
                    break;
                case "Source":
                    break;
                case "Wiki":
                    OpenRepoWikiPage();
                    break;
                case "Followers":
                    OpenRepoFollowerPage();
                    break;
                case "Stargazers":
                    OpenRepoStargazerPage();
                    break;
            }
        }

        private void OpenRepoIssuePage()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri("/MainPages/RepoIssuesPage.xaml", UriKind.Relative));
            });
        }

        private void OpenRepoCommitPage()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri("/MainPages/RepoCommitsPage.xaml", UriKind.Relative));
            });
        }

        private void OpenRepoWikiPage()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri("/InfoPages/RepoWikiPage.xaml", UriKind.Relative));
            });
        }

        private void OpenRepoFollowerPage()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri("/MainPages/RepoFollowersPage.xaml", UriKind.Relative));
            });
        }

        private void OpenRepoStargazerPage()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri("/MainPages/RepoStargazerPage.xaml", UriKind.Relative));
            });
        }
    }
}