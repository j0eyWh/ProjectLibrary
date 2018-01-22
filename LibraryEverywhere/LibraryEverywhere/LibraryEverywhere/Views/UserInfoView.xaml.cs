using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryEverywhere.Common;
using LibraryEverywhere.Models;
using LibraryEverywhere.ViewModels;
using Xamarin.Forms;
using static LibraryEverywhere.Common.UserContext;
namespace LibraryEverywhere.Views
{
    public partial class UserInfoView : ContentPage
    {
        public UserInfoView()
        {
            InitializeComponent();
            
        }

        private async void LogOffButton_OnClicked(object sender, EventArgs e)
        {
            var exit = await DisplayAlert("Log out?", "Are you sure you want to log out?", "Log out", "Cancel");
            if (!exit) return;

           //App.Current.Properties["stayLoggedIn"] = false;
            App.Current.Properties.Remove("UserCode");
            await App.Current.MainPage.Navigation.PushModalAsync(new LoginView(), true);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            userViewModel.User = new User()
            {
                Name = UserContext.CurrentUser.Name,
                Surname = UserContext.CurrentUser.Surname,
                UserPic = UserContext.CurrentUser.UserPic,
                Email = UserContext.CurrentUser.Email,
                BirthDate = CurrentUser.BirthDate,
                Idnp = CurrentUser.Idnp,
                PhoneNumber = CurrentUser.PhoneNumber

            };
        }

        private async void EditMenuItem_OnClicked(object sender, EventArgs e)
        {

            var editPage = Activator.CreateInstance(typeof (UserEditView)) as Page;
            
            await (App.Current.MainPage as MasterDetailPage)?.Detail.Navigation.PushAsync(editPage);
        }
    }
}
