using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LibraryApp.Common;
using LibraryEverywhere.Common;
using LibraryEverywhere.Models;
using Newtonsoft.Json;
using Plugin.Toasts;
using Xamarin.Forms;

namespace LibraryEverywhere.Views
{
    public partial class CategoryView : ContentPage
    {
        public string Category { get; set; }

        public CategoryView(string category)
        {
            InitializeComponent();

            this.Category = category;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CategoryViewModel.RefreshViewModel(Category);
            CategoryViewModel.Category = Category;
        }

        private async void FollowCategory_OnClicked(object sender, EventArgs e)
        {
            var cat = CategoryViewModel.Category;
            var model = new UserCateogory()
            {
                UserId = UserContext.CurrentUser.UserId,
                CategoryTitle = cat
            };


            var jsonString =  JsonConvert.SerializeObject(model);

            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            HttpResponseMessage res = null;

            var userId = UserContext.CurrentUser.UserId;

            res = await Client.CurrentInstance.PostAsync($"users/{UserContext.CurrentUser.UserId}/follow/{cat}", content);

            var notifier = DependencyService.Get<IToastNotificator>();

            if (res.IsSuccessStatusCode)
            {
                var b = await res.Content.ReadAsAsync<bool>();

                if (b)
                {
                    await notifier.Notify(ToastNotificationType.Success, "Cool!", "You just started to follow this category!", TimeSpan.FromSeconds(3));
                    return;
                }
                await notifier.Notify(ToastNotificationType.Info, "Hey!", "You already follow this category!", TimeSpan.FromSeconds(3));
                return;
            }
            await notifier.Notify(ToastNotificationType.Error, ":(", "Something bad happened!", TimeSpan.FromSeconds(3));


        }
    }
}
