using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFImageLoading.Forms;
using LibraryEverywhere.Common;
using LibraryEverywhere.Models;
using LibraryEverywhere.ViewModels;
using Xamarin.Forms;

namespace LibraryEverywhere.Views
{
    public partial class MasterPage : ContentPage
    {
        public ListView ListView => listView;
        public CachedImage ProfileImage =>this.ProfilePic;

        public ListView SecondaryListView => this.technicalListView;

        //public void UpdateUserPic(ImageSource source)
        //{
        //    this.MasterPageViewModel.UserPic = source;
        //}

        public void UpdateViewModel(User user)
        {
            this.MasterPageViewModel.Email = user.Email;
            this.MasterPageViewModel.UserName = user.Name;
            this.MasterPageViewModel.UserPic = user.ImageSource;
        }

        public MasterPage()
        {
            InitializeComponent(); 
        }

        
    }
}
