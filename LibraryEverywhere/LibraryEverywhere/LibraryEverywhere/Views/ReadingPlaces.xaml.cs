using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LibraryApp.Common;
using LibraryEverywhere.Common;
using LibraryEverywhere.Models;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace LibraryEverywhere.Views
{
    public partial class ReadingPlaces : ContentPage
    {
        public ReadingPlaces()
        {
            InitializeComponent();
            Map = new Map();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var r = await Client.CurrentInstance.GetAsync($"users/{UserContext.CurrentUser.UserId}/suitablePosition");

            if (r.IsSuccessStatusCode)
            {
                var readingPlaces = await r.Content.ReadAsAsync<IEnumerable<ReadingPosition>>();

                foreach (var readingPosition in readingPlaces)
                {
                    var pin = new Pin()
                    {
                        Position = new Position(readingPosition.Latitude, readingPosition.Longitude),
                        Type = PinType.Place,
                        Label = readingPosition.BookCode.Book.Title,
                        Address = readingPosition.User.Name + " "+readingPosition.User.Surname
                    };
                    this.Map.Pins.Add(pin);
                }
            }

        }
    }
}
