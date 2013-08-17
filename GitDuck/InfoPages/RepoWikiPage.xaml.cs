using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GitDuck.HelperClasses;
using Newtonsoft.Json;
using System.Text;

namespace GitDuck.InfoPages
{
    public partial class RepoWikiPage : PhoneApplicationPage
    {
        public RepoWikiPage()
        {
            InitializeComponent();
        }

        private void wikiTextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            wikiBusyIndicator.IsRunning = true;
            WebClient client = new WebClient();
            client.DownloadStringCompleted += clientStringDownloaded;
            client.DownloadStringAsync(new System.Uri("https://api.github.com/repos/" + (App.Current as App).CurrentRepoInfo.full_name + "/readme"));
        }

        private void clientStringDownloaded(object sender, DownloadStringCompletedEventArgs e)
        {
            FileData deserialized = JsonConvert.DeserializeObject<FileData>(e.Result);
            byte[] byteArray = System.Convert.FromBase64String(deserialized.content);
            wikiTextBlock.Text = UTF8Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
            wikiBusyIndicator.IsRunning = false;
        }
    }
}