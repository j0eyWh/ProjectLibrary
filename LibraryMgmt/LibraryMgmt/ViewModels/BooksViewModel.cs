using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryMgmt.ServiceReference;
using System.ComponentModel;
using LibraryMgmt;
using System.Windows;
using LibraryMgmt.Common;

namespace LibraryMgmt.ViewModels
{
    public class BooksViewModel : ObservableBase
    {

        private ObservableCollection<Book> _bookList = new ObservableCollection<Book>();
        public ObservableCollection<Book> BookList
        {
            get;
            set;
        }

        private ObservableCollection<Book> _searchResults;
        public ObservableCollection<Book> SearchResults
        {
            get { return _searchResults; }
            set { _searchResults = value; }
        }


        private Book _selectedBook;
        public Book SelectedBook
        {
            get { return _selectedBook; }
            set
            {
                _selectedBook = value;
                if (_selectedBook != null)
                {
                    GetAvailableAmount();
                }
                else 
                {
                    _selectedBookAvailableAmount = 0;
                }
                NotifyPropertyChanged(nameof(SelectedBook));
                NotifyPropertyChanged(nameof(SelectedBookAvailableAmount));
            }
        }

        private int _selectedBookAvailableAmount;

        public int SelectedBookAvailableAmount
        {
            get { return _selectedBookAvailableAmount; }
            set { _selectedBookAvailableAmount = value; NotifyPropertyChanged(nameof(SelectedBookAvailableAmount)); }
        }


        //CONSTRUCTOR
        public BooksViewModel()
        {
            BookList = new ObservableCollection<Book>();   
        }

        

        public async Task InitializeAsyc()
        {
            foreach (var item in await GetBooksAsync())
            {
                BookList.Add(item);
            }
            
        }

        private async Task<IEnumerable<Book>> GetBooksAsync()
        {
            return await Client.GetAllBooksAsync();
        }

        private async void GetAvailableAmount()
        {
            var x = await Client.GetBookCodesAsync(this._selectedBook);
            var y = await Client.GetBookCodesOnHandAsync(this._selectedBook);
            _selectedBookAvailableAmount = x.Length - y.Length;
        }


    }
}
