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
using System.IO.IsolatedStorage;
using System.IO;
using Newtonsoft.Json;

namespace GitDuck
{
    public partial class IssuePage : PhoneApplicationPage
    {
        private ObservableCollection<IssueData> issueItems;

        public IssuePage()
        {
            InitializeComponent();
        }

        private void issuesListBox_Loaded(object sender, RoutedEventArgs e)
        {
            issuesBusyIndicator.IsRunning = true;
            issueItems = new ObservableCollection<IssueData>();

            System.Uri myUri = new System.Uri("https://api.github.com/user/issues");
            HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(myUri);
            myRequest.Headers["Authorization"] = "Token " + IsolatedStorageSettings.ApplicationSettings["oauthToken"];
            myRequest.BeginGetResponse(new AsyncCallback(GetIssuesCallback), myRequest);
            System.Diagnostics.Debug.WriteLine(IsolatedStorageSettings.ApplicationSettings["oauthToken"]);
        }

        private void GetIssuesCallback(IAsyncResult ar)
        {
            HttpWebRequest request = (HttpWebRequest)ar.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(ar);
            StreamReader httpWebStreamReader = new StreamReader(response.GetResponseStream());
            string result = httpWebStreamReader.ReadLine();

            List<IssueData> deserialized = JsonConvert.DeserializeObject<List<IssueData>>(result);

            foreach (IssueData issueData in deserialized)
            {
                issueItems.Add(issueData);
            }

            Dispatcher.BeginInvoke(() =>
            {
                issuesListBox.ItemsSource = issueItems;
                if (issueItems.Count < 1)
                {
                    issuesListBox.EmptyContent = "You do not have any Issues!";
                }

                issuesBusyIndicator.IsRunning = false;
            });

            
        }
    }
}