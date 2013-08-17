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
using System.IO.IsolatedStorage;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using GitDuck.HelperClasses;

namespace GitDuck
{
    public partial class LoginPage : PhoneApplicationPage
    {
        public LoginPage()
        {
            InitializeComponent();
            busyIndicator.IsRunning = true;
            
            linkBtn.Visibility = System.Windows.Visibility.Collapsed;
            resetBtn.Visibility = System.Windows.Visibility.Collapsed;

            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                MessageBox.Show("No Network Connection Available! This App Requires an active network Connection!");
            }
        }

        private void linkBtn_Click(object sender, RoutedEventArgs e)
        {
            linkBtn.IsEnabled = false;
            NavigationService.Navigate(new Uri("/OAuthWebPage.xaml", UriKind.Relative));
        }

        private void resetBtn_Click(object sender, RoutedEventArgs e)
        {
            ResetLinkage();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            linkBtn.IsEnabled = true;
            if (IsolatedStorageSettings.ApplicationSettings.Contains("oauthToken"))
            {
                CheckAuthToken(IsolatedStorageSettings.ApplicationSettings["oauthToken"].ToString());

                linkBtn.IsEnabled = false;
            }
            else
            {
                busyIndicator.IsRunning = false;
                resetBtn.IsEnabled = false;
                linkBtn.Visibility = System.Windows.Visibility.Visible;
                resetBtn.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void CheckAuthToken(String str)
        {
            System.Uri myUri = new System.Uri("https://api.github.com/user");
            HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(myUri);
            myRequest.Headers["Authorization"] = "Token " + IsolatedStorageSettings.ApplicationSettings["oauthToken"];
            myRequest.BeginGetResponse(new AsyncCallback(GetValidateAuthCallback), myRequest);
        }

        private void GetValidateAuthCallback(IAsyncResult ar)
        {
            HttpWebResponse response = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)ar.AsyncState;
                response = (HttpWebResponse)request.EndGetResponse(ar);
            }
            catch (Exception ex)
            { ResetLinkage(); return; }
            StreamReader httpWebStreamReader = new StreamReader(response.GetResponseStream());
            string result = httpWebStreamReader.ReadLine();

            (App.Current as App).CurrentUserInfo = JsonConvert.DeserializeObject<User>(result);

            if ((App.Current as App).CurrentUserInfo.login != null)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri("/HomePage.xaml", UriKind.Relative));
                });
            }
            else
            {
                ResetLinkage();
            }
        }

        private void ResetLinkage()
        {
            IsolatedStorageSettings.ApplicationSettings.Remove("oauthToken");
            Dispatcher.BeginInvoke(() =>
            {
                linkBtn.Visibility = System.Windows.Visibility.Visible;
                resetBtn.Visibility = System.Windows.Visibility.Visible;
                linkBtn.IsEnabled = true;
                resetBtn.IsEnabled = false;
                busyIndicator.IsRunning = false;
            });
        }
    }
}
