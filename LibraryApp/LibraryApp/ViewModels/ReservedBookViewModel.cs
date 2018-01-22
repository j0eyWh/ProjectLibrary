using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using LibraryApp.SubServiceLayer;

namespace LibraryApp.ViewModels
{
    public class ReservedBookViewModel:INotifyPropertyChanged
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

        private ReservedBook _book = new ReservedBook();
        public ReservedBook Book
        {
            get { return _book; }
            set { _book = value; NotifyPropertyChanged(nameof(Book)); NotifyPropertyChanged(nameof(IsDue));}
        }


        public Windows.UI.Xaml.Visibility IsDue
        {
            get
            {
                var t = Book.TimeOut - DateTime.Now;
                if (t<= TimeSpan.FromDays(1))
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }


    }
}
