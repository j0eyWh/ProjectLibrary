using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryEverywhere.Models;
using Xamarin.Forms;

namespace LibraryEverywhere.Views
{
    public partial class OwnedView : ContentPage
    {
        public OwnedView()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await OwnedViewModel.RefreshViewModel();
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            this.listView.SelectedItem = null;
        }

        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            
            var details = (App.Current.MainPage as MasterDetailPage).Detail;
            //App.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(page));
           details.Navigation.PushAsync(new OwnedDetailsView(e.Item as OnHand));
        }
    }
}
