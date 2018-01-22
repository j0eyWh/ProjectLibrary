using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LibraryApp.Common;
using LibraryEverywhere.Common;
using LibraryEverywhere.Models;
using LibraryEverywhere.Views;
using Xamarin.Forms;

namespace LibraryEverywhere
{
    public partial class LoginView : ContentPage
    {
        public LoginView()
        {
            InitializeComponent();
            Navigation.RemovePage(this);
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            activityIndicator.IsRunning = true;

            var userName = LogInEntry.Text;
            var idnp = IdnpEntry.Text;
            //var stayLoggedIn = StayCb.Checked || Device.OS==TargetPlatform.iOS;
            var stayLoggedIn = true;
            var c = Client.CurrentInstance;
            var request = await Client.CurrentInstance.GetAsync($"users/canlogin/{userName}/{idnp}");
            if (request.IsSuccessStatusCode)
            {
                var canLogIn = await request.Content.ReadAsAsync<bool>();

                if (canLogIn)
                {
                    App.Current.Properties["UserCode"] = idnp;
                    activityIndicator.IsRunning = false;
                    BuildUserContext(idnp);
                    RedirectToMainPage(stayLoggedIn); 
                }
                else
                {
                    await DisplayAlert("Unable to log you in!", "User with these credentials not found.", "OK");
                }
            }
            else if (request.StatusCode==HttpStatusCode.NotFound)
            {
                await DisplayAlert("Unable to log you in!", "Some of your credentials are not correct.", "OK");
            }
            else
            {
                await DisplayAlert("Connection error", "Service unavailable", "OK");
            }
            activityIndicator.IsRunning = false;
        }

        private void RedirectToMainPage(bool stayLoggedIn)
        {
            object stay;
            if (App.Current.Properties.TryGetValue("stayLoggedIn", out stay))
            {
                App.Current.Properties["stayLoggedIn"] = stayLoggedIn;
            }
            else
            {
                App.Current.Properties.Add("stayLoggedIn", stayLoggedIn);
            }

            App.Current.MainPage = new MainPage();
        }

        private async void BuildUserContext(string idnp)
        {
            var r = Client.CurrentInstance.GetAsync($"users/user/idnp/{idnp}").Result;
            if (!r.IsSuccessStatusCode) return;
            var user = await r.Content.ReadAsAsync<User>();
            new UserContext(user);
        }

        private void Easter(object sender, EventArgs args)
        {
            IdnpEntry.Text = "1111111111111";
            LogInEntry.Text = "a.t@k.com";
        }

        private void IdnpEntry_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length>13)
            {
                ((Entry) sender).Text = e.NewTextValue.Substring(0, 13);
            }
        }
    }
}
