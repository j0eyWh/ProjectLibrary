using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using LibraryApp.SubServiceLayer;

namespace LibraryApp.ViewModels
{
    public class OnHandsBookViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private OnHand _book;

        public OnHand Book
        {
            get { return _book; }
            set { _book = value; NotifyPropertyChanged(nameof(Book)); NotifyPropertyChanged(nameof(IsDue));}
        }

        public Windows.UI.Xaml.Visibility IsDue
        {
            get
            {
                var t = Book.ReturnDate - DateTime.Now;
                if (t <= TimeSpan.FromDays(1))
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }

    }
}
