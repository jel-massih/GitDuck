﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace GitDuck
{
    public class RepoData : INotifyPropertyChanged
    {
        public int id { get; set; }
        public string name { get; set; }
        public string full_name { get; set; }
        public User owner { get; set; }
        public bool @private { get; set; }
        public string html_url { get; set; }
        public string description { get; set; }
        public bool fork { get; set; }
        public string url { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string pushed_at { get; set; }
        public string homepage { get; set; }
        public int size { get; set; }
        public int watchers_count { get; set; }
        public string language { get; set; }
        public bool has_issues { get; set; }
        public bool has_downloads { get; set; }
        public bool has_wiki { get; set; }
        public int forks_count { get; set; }
        public object mirror_url { get; set; }
        public int open_issues_count { get; set; }
        public int forks { get; set; }
        public int open_issues { get; set; }
        public int watchers { get; set; }
        public string master_branch { get; set; }
        public string default_branch { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

<<<<<<< HEAD:GitDuck/RepoData.cs

=======
>>>>>>> Added RepoIssues Page:GitDuck/HelperClasses/RepoData.cs
    public class User : INotifyPropertyChanged
    {
        public string login { get; set; }
        public int id { get; set; }
        public string avatar_url { get; set; }
        public string gravatar_id { get; set; }
        public string url { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string company { get; set; }
        public string blog { get; set; }
        public string location { get; set; }
        public string email { get; set; }
        public bool hireable { get; set; }
        public string bio { get; set; }
        public int public_repos { get; set; }
        public int followers { get; set; }
        public int following { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public int public_gists { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class RepoHubListItem : INotifyPropertyChanged
    {
        public RepoHubListItem(string itemName, string iconUrl)
        {
            this.itemName = itemName;
            this.iconUrl = iconUrl;
        }

        public string itemName { get; set; }
        public string iconUrl { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}