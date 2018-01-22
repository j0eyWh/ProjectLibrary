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
    public class BookSearchViewModel:INotifyPropertyChanged
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

        private SubServiceLayerClient _service = new SubServiceLayerClient();



        private string _searchString = null;

        public string SearchString
        {
            get { return _searchString; }
            set { _searchString = value; NotifyPropertyChanged(nameof(SearchString)); SearchBooks(value); }
            
        }

        private ObservableCollection<Book> _books = new ObservableCollection<Book>();

        public ObservableCollection<Book> Books
        {
            get { return _books; }
            set { _books = value; NotifyPropertyChanged(nameof(Books)); }
        }

        private async void SearchBooks(string searchString)
        {
            Books.Clear();

            if (!String.IsNullOrEmpty(searchString))
            {
                //Books.Clear();
                Visibility = Visibility.Visible;
                var bookList = await _service.SearchBooksAsync(searchString);
                Books = bookList;
            }
            else
            {
                Books.Clear();
            }

            Visibility = Visibility.Collapsed;

        }

        private Windows.UI.Xaml.Visibility _visibility = Windows.UI.Xaml.Visibility.Collapsed;
        public Windows.UI.Xaml.Visibility Visibility
        {
            get { return _visibility; }
            set { _visibility = value; NotifyPropertyChanged(nameof(Visibility)); }
        }


    }
}
