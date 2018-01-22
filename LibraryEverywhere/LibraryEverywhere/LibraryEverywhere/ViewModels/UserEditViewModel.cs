using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LibraryEverywhere.Common;
using LibraryEverywhere.Models;
using Xamarin.Forms;

namespace LibraryEverywhere.ViewModels
{
    public class UserEditViewModel : ObservableBase
    {
        private User _user = UserContext.CurrentUser;

        public User User
        {
            get { return _user; }
            set { _user = value; NotifyPropertyChanged(nameof(User)); }
        }

    }
}
