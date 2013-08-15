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
using Newtonsoft.Json;


namespace GitDuck
{
    public partial class GistPage : PhoneApplicationPage
    {
        private ObservableCollection<GistData> gistItems;

        public GistPage()
        {
            InitializeComponent();
        }

        private void gistsListBox_Loaded(object sender, RoutedEventArgs e)
        {
            gistsBusyIndicator.IsRunning = true;
            gistItems = new ObservableCollection<GistData>();
            gistsListBox.ItemsSource = gistItems;
            WebClient client = new WebClient();
            client.DownloadStringCompleted += client_gistsDownloadStringCompleted;
            client.DownloadStringAsync(new System.Uri("https://api.github.com/users/" + (App.Current as App).CurrentUserInfo.login + "/gists"));
        }

        private void client_gistsDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            List<GistData> deserialized = JsonConvert.DeserializeObject<List<GistData>>(e.Result);

            foreach (GistData gistData in deserialized)
            {
                string filePath = gistData.files.ToString().Substring(gistData.files.ToString().IndexOf(':') + 2);
                filePath = filePath.Substring(0, filePath.Length - 1);
                gistData.fileData = JsonConvert.DeserializeObject<GistFileData>(filePath);
                gistItems.Add(gistData);
            }

            gistsListBox.ItemsSource = gistItems;

            if (gistItems.Count < 1)
            {
                gistsListBox.EmptyContent = "You do not have any Gists!";
            }

            gistsBusyIndicator.IsRunning = false;
        }
    }
}