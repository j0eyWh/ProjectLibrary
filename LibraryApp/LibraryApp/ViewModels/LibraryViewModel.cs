using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using LibraryApp.Models;
using LibraryApp.SubServiceLayer;

namespace LibraryApp.ViewModels
{
    public class LibraryViewModel : INotifyPropertyChanged
    {
        #region Interface implementation
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        private ObservableCollection<BookCategoryModel> _booksByCategory = new ObservableCollection<BookCategoryModel>();

        public ObservableCollection<BookCategoryModel> BooksByCategory
        {
            get { return _booksByCategory; }
            set { _booksByCategory = value; }
        }

        private ObservableCollection<Book> _books;

        public ObservableCollection<Book> Books
        {
            get { return _books; }
            set { _books = value; }
        }

        private Windows.UI.Xaml.Visibility _visibility = Windows.UI.Xaml.Visibility.Visible;

        public Windows.UI.Xaml.Visibility Visibility
        {
            get { return _visibility; }
            set { _visibility = value; NotifyPropertyChanged(nameof(Visibility)); }
        }



    }
}
