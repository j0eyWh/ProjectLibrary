using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LibraryApp.Common;
using LibraryEverywhere.Common;
using LibraryEverywhere.Models;
using LibraryEverywhere.ViewModels;
using Plugin.Geolocator;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Toasts;
using Xamarin.Forms;
using static LibraryEverywhere.Common.UserContext;

namespace LibraryEverywhere.Views
{
    public partial class UserEditView : ContentPage
    {
        public UserEditView()
        {
            InitializeComponent();
            this.UserEditViewModel.User = new User()
            {
                Name = UserContext.CurrentUser.Name,
                Surname = UserContext.CurrentUser.Surname,
                UserPic = UserContext.CurrentUser.UserPic,
                Email = UserContext.CurrentUser.Email,
                BirthDate = CurrentUser.BirthDate,
                Idnp = CurrentUser.Idnp,
                PhoneNumber = CurrentUser.PhoneNumber,
                UserId = CurrentUser.UserId

            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
        }

        private async void UserPicTapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            var coolPhoto = await CrossMedia.Current.PickPhotoAsync();

            if (coolPhoto!=null)
            {
                var stream = coolPhoto.GetStream();
                UserEditViewModel.User.UserPic = stream.ToByteArray();
                UserEditViewModel.User.ImageSource = ImageSource.FromStream(coolPhoto.GetStream);
            }

        }


        private async void SaveChanges_ButtonClicked(object sender, EventArgs e)
        {
            var updatedUser = UserEditViewModel.User;
            var response = await Client.CurrentInstance.Post("users/edit", new
            {
                updatedUser.Name,
                updatedUser.Surname,
                updatedUser.UserPic,
                updatedUser.BirthDate,
                updatedUser.Email,
                updatedUser.PhoneNumber,
                updatedUser.UserId,
                updatedUser.Idnp
            });

            var isUpdated = await response.Content.ReadAsAsync<bool>();

            var notifier = DependencyService.Get<IToastNotificator>();

            if (isUpdated)
            {
                App.Current.MainPage.IsBusy = true;
                new UserContext(updatedUser);

                ((App.Current.MainPage as MainPage).Master as MasterPage).UpdateViewModel(updatedUser);


                (App.Current.MainPage as MasterDetailPage).Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(UserInfoView)));

                await notifier.Notify(ToastNotificationType.Success, "Updated!", "Info about you was successfuly updated",
                    TimeSpan.FromSeconds(3));

                App.Current.MainPage.IsBusy = false;

            }
            else
            {
                await notifier.Notify(ToastNotificationType.Error, "Ooops!", "Something bad happened! :(",
                    TimeSpan.FromSeconds(3));
            }

         
        }
    }
}
