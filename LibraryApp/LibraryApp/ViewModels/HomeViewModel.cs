using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using LibraryApp.SubServiceLayer;

namespace LibraryApp.ViewModels
{
    public class HomeViewModel:INotifyPropertyChanged
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

        public HomeViewModel()
        {
            this.RecommendedBooks.CollectionChanged += UpdateVisibilityStatus;
        }

        private void UpdateVisibilityStatus(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (RecommendedBooks.Any())
            {
                this.IsEmpty = Visibility.Collapsed;
            }
            else
            {
                this.IsEmpty = Visibility.Visible;
            }
        }

        private ObservableCollection<Book> _newestBooks = new ObservableCollection<Book>();

        public ObservableCollection<Book> NewestBooks
        {
            get { return _newestBooks; }
            set { _newestBooks = value; }
        }

        private ObservableCollection<Book> _recommendedBooks = new ObservableCollection<Book>();

        public ObservableCollection<Book> RecommendedBooks
        {
            get { return _recommendedBooks; }
            set { _recommendedBooks = value; IsEmpty=Visibility.Collapsed; NotifyPropertyChanged(nameof(IsEmpty)); }
        }

        

        private Windows.UI.Xaml.Visibility isEmpty = Visibility.Visible;

        public Windows.UI.Xaml.Visibility IsEmpty
        {
            get { return isEmpty; }
            set { isEmpty = value;
                NotifyPropertyChanged(nameof(IsEmpty));
            }
        }


    }
}
