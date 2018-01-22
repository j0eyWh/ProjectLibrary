using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace LibraryEverywhere.Views
{
    public partial class FollowedCategories : ContentPage
    {
        public FollowedCategories()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await FollowedCategoriesViewModel.RefreshViewModel();

        }
    }
}
