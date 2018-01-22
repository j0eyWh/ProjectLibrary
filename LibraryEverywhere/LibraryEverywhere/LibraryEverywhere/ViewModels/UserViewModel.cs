using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryEverywhere.Common;
using LibraryEverywhere.Models;
using Xamarin.Forms;

namespace LibraryEverywhere.ViewModels
{
    

    public class UserViewModel:INotifyPropertyChanged
    {
        private User _user = new User() {Name = "What a name"};

        public User User
        {
            get { return _user; }
            set { _user = value; NotifyPropertyChanged(nameof(User));}
        }


        private DateTime date = DateTime.Now;

        public DateTime Date
        {
            get { return date; }
            set { date = value; NotifyPropertyChanged(nameof(Date));}
        }


        private string s = null;

        public string S
        {
            get { return s; }
            set { s = value; NotifyPropertyChanged(nameof(S)); }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }



    }
}
