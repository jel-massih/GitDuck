using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace GitDuck
{
    public class GistData : INotifyPropertyChanged
    {
        public string url { get; set; }
        public string html_url { get; set; }
        public bool @public { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string description { get; set; }
        public int comments { get; set; }
        public User user { get; set; }
        public string comments_url { get; set; }
        public object files { get; set; }
        public GistFileData fileData { get; set; }
        
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

    public class GistFileData : INotifyPropertyChanged
    {
        public string filename { get; set; }
        public string type { get; set; }
        public object language { get; set; }
        public string raw_url { get; set; }
        public int size { get; set; }

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
