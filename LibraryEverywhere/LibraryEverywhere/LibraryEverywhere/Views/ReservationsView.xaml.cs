using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace LibraryEverywhere.Views
{
    public partial class ReservationsView : ContentPage
    {
        public ReservationsView()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await ReservationsViewModel.RefreshViewModel();
        }
    }
}
