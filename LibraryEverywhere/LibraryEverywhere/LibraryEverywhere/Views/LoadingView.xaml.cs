using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FFImageLoading.Transformations;
using LibraryApp.Common;
using LibraryEverywhere.Common;
using LibraryEverywhere.Models;
using Xamarin.Forms;

namespace LibraryEverywhere.Views
{
    enum UserContextBuildStates
    {
        NoIdnpFound = 1,
        Succedd = 2,
        HttpError = 3
    }
    public partial class LoadingView : ContentPage
    {
        public LoadingView()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            StatusLabel.Text = "Building HttpClient...";

            var f = await (new Client()).GetAccessToken();

            if (f)
            {
                StatusLabel.Text = "Building UserContext...";

                var s = await BuildUserContext();

                switch (s)
                {
                    case UserContextBuildStates.NoIdnpFound:
                        { App.Current.MainPage = new LoginView(); return; }

                    case UserContextBuildStates.Succedd:
                        { App.Current.MainPage = new MainPage();
                          return;
                        }
                    case UserContextBuildStates.HttpError:
                    {
                        await DisplayAlert("Something went wrong!", ":(", "OK"); return;
                    }
                }
            }
            await DisplayAlert("Connection failed!", ":(", "OK"); 
           
        }

        private async Task<UserContextBuildStates> BuildUserContext()
        {
            object userCode;
            if (!App.Current.Properties.TryGetValue("UserCode", out userCode)) return UserContextBuildStates.NoIdnpFound;

            var idnp = userCode as string;
            if (idnp != null)
            {
                try
                {
                    var r = await Client.CurrentInstance.GetAsync($"users/user/idnp/{idnp}");
                    if (!r.IsSuccessStatusCode) return UserContextBuildStates.HttpError;
                    var user = await r.Content.ReadAsAsync<User>();
                    new UserContext(user);
                    return UserContextBuildStates.Succedd;
                }
                catch (Exception)
                {
                    return UserContextBuildStates.HttpError;
                }
            }
            else
            {
                return UserContextBuildStates.HttpError;
            }
        }
    }
}
