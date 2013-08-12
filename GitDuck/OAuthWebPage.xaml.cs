using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO;
using System.IO.IsolatedStorage;

namespace GitDuck
{
    public partial class OAuthWebPage : PhoneApplicationPage
    {
        bool bLoginCheck = true;

        public OAuthWebPage()
        {
            InitializeComponent();
            busyIndicator.IsRunning = true;
            Uri oauthUri = new Uri("https://github.com/logout/");
            oauthBrowser.Navigated += oauthBrowser_Navigated;
            oauthBrowser.Navigating += oauthBrowser_Navigating;
            oauthBrowser.Navigate(oauthUri);
        }

        void oauthBrowser_Navigating(object sender, NavigatingEventArgs e)
        {
            busyIndicator.IsRunning = true;
        }

        void oauthBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            bool bSignedIn = false;

            CookieCollection collection = oauthBrowser.GetCookies();
            foreach (Cookie c in collection)
            {
                if (c.Name == "logged_in" && c.Value == "yes")
                {
                    bSignedIn = true;
                }
            }

            if (bSignedIn && bLoginCheck)
            {
                busyIndicator.IsRunning = false;

                return;
            }
            else if (bLoginCheck)
            {
                bLoginCheck = false;
                Uri oauthUri = new Uri("https://github.com/login/oauth/authorize?client_id=f58c3fa6caa3c8f9f8a2&scope=user,repo", UriKind.Absolute);
                oauthBrowser.Navigate(oauthUri);
                return;
            }

            busyIndicator.IsRunning = false;

            String uriString = e.Uri.ToString();

            if (uriString.LastIndexOf("?code=") != -1)
            {
                busyIndicator.IsRunning = true;
                String code = uriString.Substring(uriString.LastIndexOf("code=") + 5);
                System.Uri myUri = new System.Uri("https://github.com/login/oauth/access_token?client_id=f58c3fa6caa3c8f9f8a2&client_secret=d12f9709f99ce5948ca5c69e43b8bc5dbee66832&code=" + code);
                HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(myUri);
                myRequest.Method = "POST";
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.BeginGetResponse(new AsyncCallback(GetResponseStreamCallback), myRequest);
            }
        }

        private void GetResponseStreamCallback(IAsyncResult ar)
        {
            HttpWebRequest request = (HttpWebRequest)ar.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(ar);
            StreamReader httpWebStreamReader = new StreamReader(response.GetResponseStream());
            string result = httpWebStreamReader.ReadToEnd();
            string[] data = result.Split('=');

            string token = data[1].Substring(0, data[1].IndexOf('&'));

            if (token != "")
            {
                IsolatedStorageSettings.ApplicationSettings.Add("oauthToken", token);
            }
            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
            });
        }
    }
}