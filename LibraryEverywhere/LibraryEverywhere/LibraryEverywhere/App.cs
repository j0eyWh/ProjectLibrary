using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LibraryApp.Common;
using LibraryEverywhere.Common;
using LibraryEverywhere.Models;
using LibraryEverywhere.Views;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace LibraryEverywhere
{
    public class App : Application
    {
        public App()
        {
            MainPage = new LoadingView();
            //object stay;
            ////Build the Client instance
            //new Client().GetAccessToken();
            ////Build the User instance
            //BuildUserContext();
            ////InitEverything();
            //if (App.Current.Properties.TryGetValue("stayLoggedIn", out stay))
            //{
            //    var isRemebered = stay as bool?;
            //    if (isRemebered != null && isRemebered == true)
            //    {
            //        MainPage = new MainPage();
            //    }
            //    else
            //    {
            //        MainPage = new LoginView();
            //    }
            //}
            //else
            //{
            //    var content = new LoginView();
            //    //MainPage = new NavigationPage(content);
            //    MainPage = content;
            //}
        }

        protected override async void OnStart()
        {

        }

        private async void InitEverything()
        {
            new Client();

            const string userName = "alex.turcan@outlook.com";
            const string password = "123456Aa!";

            using (var client = new HttpClient())
            {
                //Build the Client instance
                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", userName),
                    new KeyValuePair<string, string>("password", password)
                });

                HttpResponseMessage response = await client.PostAsync($"http://10.102.0.198:35090/token", formContent);

                var responseJson = await response.Content.ReadAsStringAsync();
                var jObject = JObject.Parse(responseJson);
                var t = jObject.GetValue("access_token").ToString();
                Client.CurrentInstance.DefaultRequestHeaders.Remove("Authorization");
                Client.CurrentInstance.DefaultRequestHeaders.Add("Authorization", $"Bearer {t}");

                //Build User Context
                object userCode;
                if (!App.Current.Properties.TryGetValue("UserCode", out userCode)) return;
                var idnp = userCode as string;
                if (idnp != null)
                {
                    var r = Client.CurrentInstance.GetAsync($"users/user/idnp/{idnp}").Result;
                    if (!r.IsSuccessStatusCode) return;
                    var user = r.Content.ReadAsAsync<User>().Result;
                    new UserContext(user);
                }
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private void BuildUserContext()
        {
            object userCode;
            if (!App.Current.Properties.TryGetValue("UserCode", out userCode)) return;
            var idnp = userCode as string;
            if (idnp != null)
            {
                var r = Client.CurrentInstance.GetAsync($"users/user/idnp/{idnp}").Result;
                if (!r.IsSuccessStatusCode) return;
                var user = r.Content.ReadAsAsync<User>().Result;
                new UserContext(user);
            }
        }
    }
}
