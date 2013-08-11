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

            accountItem.Header = (App.Current as App).UserData.UserName;
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
            client.DownloadStringAsync(new System.Uri("https://github.com/"+(App.Current as App).UserData.UserName+".atom"));
        }

        private void client_feedDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            UpdateNewsFeedList(e.Result);
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
                    TextSyndicationContent content = new TextSyndicationContent(FormatTimeOffset(DateTimeOffset.Now - item.PublishDate));
                    item.Summary = content;
                    userFeedItems.Add(item);
                }
            }
            catch (Exception) { userFeedListBox.EmptyContent = "Dead"; }

            userFeedBusyIndicator.IsRunning = false;
        }

        private String FormatTimeOffset(TimeSpan timeOffset)
        {
            String time = "";

            if (timeOffset.Days != 0)
            {
                if (timeOffset.Days > 368)
                {
                    if (timeOffset.Days / 368 > 1)
                    {
                        time = timeOffset.Days / 368 + " years ago";
                    }
                    else
                    {
                        time = timeOffset.Days / 368 + " year ago";
                    }

                }
                else if (timeOffset.Days > 1)
                {
                    time = timeOffset.Days + " days ago";
                }
                else
                {
                    time = timeOffset.Days + " day ago";
                }
            }
            else if (timeOffset.Hours != 0)
            {
                if (timeOffset.Hours > 1)
                {
                    time = timeOffset.Hours + " hours ago";
                }
                else
                {
                    time = timeOffset.Hours + " hour ago";
                }
            }
            else if (timeOffset.Minutes != 0)
            {
                if (timeOffset.Minutes > 1)
                {
                    time = timeOffset.Minutes + " minutes ago";
                }
                else
                {
                    time = timeOffset.Minutes + " minute ago";
                }
            }

            return time;
        }

        private void accountItem_Loaded(object sender, RoutedEventArgs e)
        {
            accountContentContainer.DataContext = (App.Current as App).UserData;
            BitmapImage image = new BitmapImage(new Uri((App.Current as App).UserData.AvatarURL, UriKind.Absolute));
            avatarImg.Source = null;
            avatarImg.Source = image;
        }

        private void repositoryTile_Tap(object sender, GestureEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri("/RepoPage.xaml?page=repos", UriKind.Relative));
            });
        }

        private void gistTile_Tap(object sender, GestureEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri("/GistPage.xaml", UriKind.Relative));
            });
        }

        private void starTile_Tap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/RepoPage.xaml?page=starred", UriKind.Relative));
        }

        private void issuesTile_Tap(object sender, GestureEventArgs e)
        {
            System.Uri myUri = new System.Uri("https://api.github.com/user/issues");
            HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(myUri);
            myRequest.Headers["Authorization"] = "Token " + IsolatedStorageSettings.ApplicationSettings["oauthToken"];
            System.Diagnostics.Debug.WriteLine(IsolatedStorageSettings.ApplicationSettings["oauthToken"]);
            myRequest.BeginGetResponse(new AsyncCallback(GetIssuesCallback), myRequest);
        }

        private void GetIssuesCallback(IAsyncResult ar)
        {
            HttpWebRequest request = (HttpWebRequest)ar.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(ar);
            StreamReader httpWebStreamReader = new StreamReader(response.GetResponseStream());
            string result = httpWebStreamReader.ReadLine();
            System.Diagnostics.Debug.WriteLine(result);
        }
    }
}