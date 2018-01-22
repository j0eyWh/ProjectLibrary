using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LibraryApp.Common;
using LibraryEverywhere.Common;
using LibraryEverywhere.Models;

namespace LibraryEverywhere.ViewModels
{
    public class LibraryViewModel : ObservableBase
    {
        private ObservableCollection<BookCategoryModel> _groupedBooks = new ObservableCollection<BookCategoryModel>();
        public ObservableCollection<BookCategoryModel> GroupedBooks
        {
            get { return _groupedBooks; }
            set
            {
                _groupedBooks = value; NotifyPropertyChanged(nameof(GroupedBooks));
            }
        }

        public async Task RefreshModel()
        {
            List<BookCategoryModel> result = new List<BookCategoryModel>();

            var r = await Client.CurrentInstance.GetAsync("books/categories");

            if (r.IsSuccessStatusCode)
            {
                GroupedBooks.Clear();
                var books = await r.Content.ReadAsAsync<IEnumerable<Book>>();
                var booksByCategory = books.GroupBy(x => x.Category);


                foreach (var book in booksByCategory)
                {
                    var bookCategory = new BookCategoryModel();
                    foreach (var b in book.Select(x => x).ToList())
                    {
                        bookCategory.Add(b);
                    }

                    bookCategory.CategoryTitle = book.Key;
                    bookCategory.ShortTitle = book.Key.Substring(0, 2);

                    bookCategory.LastUpdated = book.Max(x => x.RegistrationDate);

                    result.Add(bookCategory);
                }

            }

            GroupedBooks = new ObservableCollection<BookCategoryModel>(result.OrderByDescending(x => x.LastUpdated));
        }
    }
}
