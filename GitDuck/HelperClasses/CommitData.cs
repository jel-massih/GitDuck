using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace GitDuck.HelperClasses
{
    public class CommitData : INotifyPropertyChanged
    {
        public string sha { get; set; }
        public Commit commit { get; set; }

        public string commitDetail { get; set; }
        public string publishDate { get; set; }

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

    public class Author
    {
        public string name { get; set; }
        public string email { get; set; }
        public string date { get; set; }
    }

    public class Commit
    {
        public Author author { get; set; }
        public string message { get; set; }
        public string url { get; set; }
        public int comment_count { get; set; }

    }
}
