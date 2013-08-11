using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace GitDuck
{
    public class UserData : INotifyPropertyChanged
    {
        private String _userName, _realName, _avatarURL, _companyName;

        public UserData()
        {
            _userName = "";
            _realName = "";
            _avatarURL = "";
            _companyName = "";
        }

        public String UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public String RealName
        {
            get { return _realName; }
            set { _realName = value; }
        }

        public String AvatarURL
        {
            get { return _avatarURL; }
            set { _avatarURL = value; }
        }

        public String CompanyName
        {
            get { return _companyName; }
            set { _companyName = value; }
        }

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
