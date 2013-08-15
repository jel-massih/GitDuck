using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace GitDuck
{
    public class IssueData : INotifyPropertyChanged
    {
        public string url { get; set; }
        public int id { get; set; }
        public int number { get; set; }
        public string title { get; set; }
        public User user { get; set; }
        public List<Label> labels { get; set; }
        public string state { get; set; }
        public User assignee { get; set; }
        public object milestone { get; set; }
        public int comments { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public object closed_at { get; set; }
        public PullRequest pull_request { get; set; }
        public RepoData repository { get; set; }
        public string body { get; set; }
        public string assignInfo { get; set; }
        public string lastUpdateTime { get; set; }

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

    public class PullRequest
    {
        public object html_url { get; set; }
        public object diff_url { get; set; }
        public object patch_url { get; set; }
    }

    public class Label
    {
        public string url { get; set; }
        public string name { get; set; }
        public string color { get; set; }
    }
}
