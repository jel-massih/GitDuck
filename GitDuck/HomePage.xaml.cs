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
using System.IO.IsolatedStorage;
using System.IO;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Text.RegularExpressions;

namespace GitDuck
{
    public partial class HomePage : PhoneApplicationPage
    {
        private ObservableCollection<SyndicationItem> userFeedItems;

        public HomePage()
        {
            InitializeComponent();

            if (NavigationContext == null || NavigationContext.QueryString == null || NavigationContext.QueryString.Count == 0)
            {
                (App.Current as App).rateReminder.Notify();
            }

            try
            {
                accountItem.Header = (App.Current as App).CurrentUserInfo.login;
            }
            catch (Exception)
            {
                MessageBox.Show("Could Not Load Current User Info");
            }
        }

        private void signOutBtn_Click(object sender, EventArgs e)
        {
            IsolatedStorageSettings.ApplicationSettings.Remove("oauthToken");
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
            });
        }

        private void userFeedListBox_Loaded(object sender, RoutedEventArgs e)
        {
            userFeedBusyIndicator.IsRunning = true;
            userFeedItems = new ObservableCollection<SyndicationItem>();
            userFeedListBox.ItemsSource = userFeedItems;
            WebClient client = new WebClient();
            client.DownloadStringCompleted += client_feedDownloadStringCompleted;
            client.DownloadStringAsync(new System.Uri("https://github.com/"+(App.Current as App).CurrentUserInfo.login+".atom"));
        }

        private void client_feedDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                UpdateNewsFeedList(e.Result);
            }
            catch
            {
                Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("Conn Issue: " + e.Error);
                });
            }
        }

        private void UpdateNewsFeedList(String atomReader)
        {
            StringReader stringReader = new StringReader(atomReader);
            XmlReader xmlReader = XmlReader.Create(stringReader);

            userFeedItems.Clear();

            try
            {
                SyndicationFeed feed = SyndicationFeed.Load(xmlReader);

                foreach (SyndicationItem item in feed.Items)
                {
                    TextSyndicationContent content = new TextSyndicationContent(HelperClasses.HelperMethods.FormatTimeOffset(DateTimeOffset.Now - item.PublishDate));
                    item.Summary = content;
                    userFeedItems.Add(item);
                }
            }
            catch (Exception) { userFeedListBox.EmptyContent = "Dead"; }

            userFeedBusyIndicator.IsRunning = false;
        }

        private void accountItem_Loaded(object sender, RoutedEventArgs e)
        {
            accountContentContainer.DataContext = (App.Current as App).CurrentUserInfo;
            BitmapImage image = new BitmapImage(new Uri((App.Current as App).CurrentUserInfo.avatar_url, UriKind.Absolute));
            avatarImg.Source = null;
            avatarImg.Source = image;
        }

        private void repositoryTile_Tap(object sender, GestureEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri("/MainPages/RepoPage.xaml?page=repos", UriKind.Relative));
            });
        }

        private void gistTile_Tap(object sender, GestureEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri("/MainPages/GistPage.xaml", UriKind.Relative));
            });
        }

        private void starTile_Tap(object sender, GestureEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri("/MainPages/RepoPage.xaml?page=starred", UriKind.Relative));
            });
        }

        private void issuesTile_Tap(object sender, GestureEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri("/MainPages/IssuePage.xaml", UriKind.Relative));
            });
        }

    }
}