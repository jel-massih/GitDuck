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
using Newtonsoft.Json;

namespace GitDuck
{
    public partial class RepoIssuesPage : PhoneApplicationPage
    {
        private ObservableCollection<IssueData> allIssueItems;
        private ObservableCollection<IssueData> bugIssueItems;
        private ObservableCollection<IssueData> enhancementIssueItems;
        private ObservableCollection<IssueData> questionIssueItems;

        public RepoIssuesPage()
        {
            InitializeComponent();
        }

        private void issuesPivot_Loaded(object sender, RoutedEventArgs e)
        {
            issuesPivot.Title = (App.Current as App).CurrentRepoInfo.name + "/Issues - Open";

            allIssueItems = new ObservableCollection<IssueData>();
            bugIssueItems = new ObservableCollection<IssueData>();
            enhancementIssueItems = new ObservableCollection<IssueData>();
            questionIssueItems = new ObservableCollection<IssueData>();

            allIssuesBusyIndicator.IsRunning = true;
            WebClient client = new WebClient();
            client.DownloadStringCompleted += client_allIssuesDownloadStringCompleted;
            client.DownloadStringAsync(new System.Uri("https://api.github.com/repos/" + (App.Current as App).CurrentRepoInfo.full_name + "/issues"));
        }

        void client_allIssuesDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            List<IssueData> deserialized = JsonConvert.DeserializeObject<List<IssueData>>(e.Result);

            foreach (IssueData issueData in deserialized)
            {
                if (issueData.assignee != null)
                {
                    issueData.assignInfo = issueData.assignee.login;
                }
                else
                {
                    issueData.assignInfo = "Unassigned";
                }
                //2013-08-14T18:47:51Z FORMAT
                DateTime dateTime = HelperClasses.HelperMethods.GitHubDateToDateTime(issueData.updated_at);
                if (dateTime != null)
                {
                    TimeSpan timeOffset = DateTimeOffset.Now - dateTime;
                    issueData.lastUpdateTime = HelperClasses.HelperMethods.FormatTimeOffset(timeOffset);
                }

                allIssueItems.Add(issueData);
            }

            LoadLabelIssues();

            allIssuesBusyIndicator.IsRunning = false;
        }

        private void LoadLabelIssues()
        {
            foreach (IssueData issueData in allIssueItems)
            {
                foreach (Label label in issueData.labels)
                {
                    switch (label.name)
                    {
                        case "bug":
                            bugIssueItems.Add(issueData);
                            break;
                        case "enhancement":
                            enhancementIssueItems.Add(issueData);
                            break;
                        case "question":
                            questionIssueItems.Add(issueData);
                            break;
                    }
                }
            }
        }

        private void allIssuesListBox_Loaded(object sender, RoutedEventArgs e)
        {
            allIssuesListBox.ItemsSource = allIssueItems;
            if (allIssueItems.Count < 1)
            {
                allIssuesListBox.EmptyContent = "No Issues Have Been Found!";
            }
        }

        private void bugsIssuesListBox_Loaded(object sender, RoutedEventArgs e)
        {
            bugsIssuesListBox.ItemsSource = bugIssueItems;
            if (bugIssueItems.Count < 1)
            {
                bugsIssuesListBox.EmptyContent = "No Bug Issues Have Been Found!";
            }
        }

        private void enhancementsIssuesListBox_Loaded(object sender, RoutedEventArgs e)
        {
            enhancementsIssuesListBox.ItemsSource = enhancementIssueItems;
            if (enhancementIssueItems.Count < 1)
            {
                enhancementsIssuesListBox.EmptyContent = "No Enhancement Issues Have Been Found!";
            }
        }

        private void questionsIssuesListBox_Loaded(object sender, RoutedEventArgs e)
        {
            questionsIssuesListBox.ItemsSource = questionIssueItems;
            if (questionIssueItems.Count < 1)
            {
                allIssuesListBox.EmptyContent = "No Question Issues Have Been Found!";
            }
        }
    }
}