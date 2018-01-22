using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Email;
using LibraryApp.SubServiceLayer;

namespace LibraryApp.ViewModels
{
    public class BookViewModel: INotifyPropertyChanged
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

        private Book _book = new Book();

        public Book Book
        {
            get { return _book; }
            set { _book = value; NotifyPropertyChanged(nameof(Book)); }
        }

        private int _availableCount;

        public int AvailableCount
        {
            get { return _availableCount; }
            set { _availableCount = value; NotifyPropertyChanged(nameof(AvailableCount)); }
        }


        public BookViewModel()
        {
            this.Book = new Book();
        }


    }
}
