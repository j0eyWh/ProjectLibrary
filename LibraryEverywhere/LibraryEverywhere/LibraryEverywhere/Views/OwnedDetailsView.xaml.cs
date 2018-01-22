using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LibraryApp.Common;
using LibraryEverywhere.Common;
using LibraryEverywhere.Models;
using Plugin.Geolocator;
using Plugin.Toasts;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Plugin.LocalNotifications;

namespace LibraryEverywhere.Views
{
    public partial class OwnedDetailsView : ContentPage
    {
        private OnHand Book { get; set; }
        public OwnedDetailsView(OnHand book)
        {
            InitializeComponent();
            this.Book = book;
        }

        protected override async void OnAppearing()
        {
            CrossLocalNotifications.Current.Show("title", "hello", 10);
            base.OnAppearing();
            OwnedDetailsViewModel.OwnedBook = Book;

            var r = await Client.CurrentInstance.GetAsync($"books/{Book.BookCode.Book.BookId}/readingPositions");

            if (r.IsSuccessStatusCode)
            {
                var positions = await r.Content.ReadAsAsync<IEnumerable<ReadingPosition>>();
                if (positions.Any())
                {
                    Map.MoveToRegion(
                        MapSpan.FromCenterAndRadius(
                            new Position(positions.FirstOrDefault().Latitude, positions.FirstOrDefault().Longitude),
                            Distance.FromKilometers(200)));
                }
                foreach (var position in positions)
                {
                    var pin = new Pin()
                    {
                        Position = new Position(position.Latitude, position.Longitude),
                        Address = position.User.Name + " "+position.User.Surname,
                        Label = Book.BookCode.Book.Title
                    };
                    Map.Pins.Add(pin);
                }
            }
        }

        private async void Check_OnClicked(object sender, EventArgs e)
        {

            var share = await DisplayAlert("Share you reading location?", "By choosing yes other users will see where you were reading this book", "Yes", "No");

            var n = DependencyService.Get<IToastNotificator>();

            if (!share) return;

            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;

                var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);

                var ownedBook = ((ToolbarItem)sender).CommandParameter as OnHand;

                if (ownedBook != null)
                {
                    var model = new ReadingPosition()
                    {
                        Longitude = position.Longitude,
                        Latitude = position.Latitude,
                        BookCodeId = ownedBook.Code,
                        SharingTime = DateTime.Now,
                        UserId = UserContext.CurrentUser.UserId
                    };

                    var r = await Client.CurrentInstance.Post("users/readingPositon", model);

                    if (r.IsSuccessStatusCode)
                    {
                        await n.Notify(ToastNotificationType.Success, "Yaay!",
                        $"We posted your position({position.Latitude};{position.Longitude}), you can see it on \"Reading positions\" page", TimeSpan.FromSeconds(5)); return;
                    }
                    await n.Notify(ToastNotificationType.Success, "Ohh snap!",
                    "We were unable to save your position :(", TimeSpan.FromSeconds(5));
                }
            }

            catch (Exception)
            {
                await n.Notify(ToastNotificationType.Success, "Ohh snap!",
                    "We were unable to get your position :(", TimeSpan.FromSeconds(5));
            }
        }
    }
}
