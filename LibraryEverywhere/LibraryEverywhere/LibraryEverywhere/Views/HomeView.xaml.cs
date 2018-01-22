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
using Plugin.Toasts;
using Xamarin.Forms;

namespace LibraryEverywhere.Views
{
    public partial class HomeView : ContentPage
    {
        public HomeView()
        {
            InitializeComponent();

            
        }

        protected override async void OnAppearing()
        {
            listView.IsRefreshing = true;
            this.IsBusy = true;
            await libraryViewModel.RefreshModel();
            listView.IsRefreshing = false;
            this.IsBusy = false;
        }

        private async void ListView_OnRefreshing(object sender, EventArgs e)
        {
            this.IsBusy = true;
            await libraryViewModel.RefreshModel();
            listView.IsRefreshing = false;
            this.IsBusy = false;
            return;
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new NavigationPage(new CategoryView()));
            var category = (e as TappedEventArgs)?.Parameter.ToString();
            var x = App.Current.MainPage as MasterDetailPage;

            x.Detail.Navigation.PushAsync(new CategoryView(category));
        }

        private async void ReserveMenuItem_OnClicked(object sender, EventArgs e)
        {
            var notificator = DependencyService.Get<IToastNotificator>();

            var book = ((MenuItem) sender).CommandParameter as Book;

            var confirm = await DisplayAlert("Confirm reservation", $"Are you sure you want to reserve \"{(book)?.Title}\"?", "Yes", "Cancel");

            if (confirm)
            {
                var r =
                   await Client.CurrentInstance.GetAsync(
                        $"users/user/{UserContext.CurrentUser.UserId}/reserve2/{book?.BookId}");

                if (r.StatusCode == HttpStatusCode.NotAcceptable)
                {
                    await
                        notificator.Notify(ToastNotificationType.Error, "Uoops!",
                            "You can't reserve a book that you already reserved!", TimeSpan.FromSeconds(2));
                    return;
                }

                if (r.StatusCode == HttpStatusCode.Conflict)
                {
                    await
                        notificator.Notify(ToastNotificationType.Error, "Uoops!",
                            "You can't reserve a book that you already own!", TimeSpan.FromSeconds(2));
                    return;
                }

                if (r.IsSuccessStatusCode)
                {
                    bool tapped =
                    await notificator.Notify(ToastNotificationType.Success, "Hooray!", $"You just reserved \"{book.Title}\"", TimeSpan.FromSeconds(7));

                    if (tapped)
                    {
                        var x = App.Current.MainPage as MasterDetailPage;
                        x.Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(ReservationsView)));
                    }
                    

                }
                
            }

        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView) sender).SelectedItem = null;
        }
    }
}
