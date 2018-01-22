using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using LibraryApp.SubServiceLayer;

namespace LibraryApp.ViewModels
{
    public class ReservedBooksViewModel : INotifyPropertyChanged
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

        private ObservableCollection<ReservedBook> _reservedBooks = new ObservableCollection<ReservedBook>();
        public ObservableCollection<ReservedBook> ReservedBooks
        {
            get { return _reservedBooks; }
            set { _reservedBooks = value; }
        }

        private bool isLoadilng = true;

        public bool IsLoading
        {
            get { return isLoadilng; }
            set { isLoadilng = value; NotifyPropertyChanged(nameof(IsLoading)); }
        }

        private Windows.UI.Xaml.Visibility _visibility = Windows.UI.Xaml.Visibility.Visible;

        public Windows.UI.Xaml.Visibility Visibility
        {
            get { return _visibility; }
            set { _visibility = value; NotifyPropertyChanged(nameof(Visibility)); }
        }




    }
}
