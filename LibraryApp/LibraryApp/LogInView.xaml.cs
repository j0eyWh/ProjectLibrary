using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using System.Threading.Tasks;
using LibraryApp.SubServiceLayer;
using Windows.Storage;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LibraryApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LogInView : Page
    {
        private SubServiceLayer.SubServiceLayerClient service;
        ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        public LogInView()
        {
            this.InitializeComponent();
            service = new SubServiceLayerClient();
        }

        private async void LogInBtn_Click(object sender, RoutedEventArgs e)
        {
            //Frame.Navigate(typeof(MainPage), "there you go");
            MessageDialog m = new MessageDialog("title");
            string name = userNameTb.Text;
            string idnp = idnpTb.Text;

            ProgressRing.IsActive = true;

            bool canLogIn = await service.CanLogInAsync(name, idnp);

            if (canLogIn)
            {
                if ((bool)RememberCb.IsChecked)
                {
                    localSettings.Values["RememberUser"] = "true";
                }

                localSettings.Values["UserCode"] = idnp;

                User u = await service.GetUserAsync(localSettings.Values["UserCode"].ToString());
                localSettings.Values["UserId"] = u.UserId;

                UserContext.CurrentUser = u;

                ProgressRing.IsActive = false;
                Frame.Navigate(typeof(MainPage));
            }
            else
            {
                m.Content = $"An user with this name {name} and idnp: {idnp} is not registered";
                m.Title = "User not found";
                await m.ShowAsync();
                ProgressRing.IsActive = false;
            }

            



        }
    }
}
