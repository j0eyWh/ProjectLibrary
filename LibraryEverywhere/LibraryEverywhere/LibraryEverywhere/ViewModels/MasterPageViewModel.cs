using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFImageLoading.Work;
using LibraryEverywhere.Common;
using LibraryEverywhere.Models;
using LibraryEverywhere.Views;
using ImageSource = Xamarin.Forms.ImageSource;

namespace LibraryEverywhere.ViewModels
{
    public class MasterPageViewModel:ObservableBase
    {
        public MasterPageViewModel()
        {
            var homeMenuItem = new MasterPageItem()
            {
                Title = "Home",
                IconSource = "home.png",
                TargetType = typeof(HomeView)
            };

            var profileMenuItem = new MasterPageItem()
            {
                Title = "My Profile",
                IconSource = "user.png",
                TargetType = typeof(UserInfoView)
            };

            var spotlightMenuItem = new MasterPageItem()
            {
                Title = "Interesting",
                IconSource = "star.png",
                TargetType = typeof(SpotlightView)
            };

            var reservationsMenuItem = new MasterPageItem()
            {
                Title = "Reservations",
                IconSource = "ticket.png",
                TargetType = typeof(ReservationsView)
            };

            var ownedMenuItem = new MasterPageItem()
            {
                Title = "Owned",
                IconSource = "books.png",
                TargetType = typeof(OwnedView)
            };

            var followedMenuItem = new MasterPageItem()
            {
                Title = "Following",
                IconSource = "heart.png",
                TargetType = typeof(FollowedCategories)
            };

            var placesMenuItem = new MasterPageItem()
            {
                Title = "Places",
                IconSource = "place.png",
                TargetType = typeof(ReadingPlaces)
            };

            var settingsMenuItem = new MasterPageItem()
            {
                Title = "Settings",
                IconSource = "settings.png",
                TargetType = typeof(SettingsView)
            };

            _items.Add(homeMenuItem);
            _items.Add(spotlightMenuItem);
            _items.Add(followedMenuItem);
            _items.Add(reservationsMenuItem);
            _items.Add(ownedMenuItem);
            _items.Add(placesMenuItem);
            _technicalItems.Add(profileMenuItem);
            _technicalItems.Add(settingsMenuItem);

        }

        private ObservableCollection<MasterPageItem> _items = new ObservableCollection<MasterPageItem>();

        public ObservableCollection<MasterPageItem> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        private ObservableCollection<MasterPageItem> _technicalItems = new ObservableCollection<MasterPageItem>();

        public ObservableCollection<MasterPageItem> TechnicalItems
        {
            get { return _technicalItems; }
            set { _technicalItems = value; }
        }

        private ImageSource _userPic = UserContext.CurrentUser.ImageSource;

        public ImageSource UserPic
        {
            get
            {
                return _userPic;
            }
            set
            {
                _userPic = value;
                NotifyPropertyChanged(nameof(UserPic));
            }
        }


        private string _userName = UserContext.CurrentUser.Name;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; NotifyPropertyChanged(nameof(UserName)); }
        }


        //  public string UserName => UserContext.CurrentUser.Name;

        private string _email = UserContext.CurrentUser.Email;

        public string Email
        {
            get { return _email; }
            set { _email = value; NotifyPropertyChanged(nameof(Email)); }
        }
        
    }
}
