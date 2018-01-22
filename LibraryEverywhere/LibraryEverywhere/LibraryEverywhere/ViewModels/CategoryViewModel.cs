using System;
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
    public class CategoryViewModel:ObservableBase
    {
        private ObservableCollection<Category<Book>> _books = new ObservableCollection<Category<Book>>();

        public ObservableCollection<Category<Book>> Books
        {
            get { return _books; }
            set { _books = value; NotifyPropertyChanged(nameof(Books)); }
        }

        private string _category;

        public string Category
        {
            get { return _category; }
            set { _category = value; NotifyPropertyChanged(nameof(Category)); }
        }


        public async Task RefreshViewModel(string category)
        {
            Category = category;
            var r = await Client.CurrentInstance.GetAsync($"books/{category}");

            if (r.IsSuccessStatusCode)
            {
                var books = await r.Content.ReadAsAsync<IEnumerable<Book>>();

                var groupedBooks = books.GroupBy(x => x.Title[0]);

                List<Category<Book>> temp = new List<Category<Book>>();

                foreach (var groupedBook in groupedBooks)
                {
                    var b = new Category<Book>()
                    {
                        Name = groupedBook.Key.ToString(),
                        ShortName = groupedBook.Key.ToString()
                    };

                    foreach (var book in groupedBook)
                    {
                        b.Add(book);
                    }

                    temp.Add(b);
                }

                Books = new ObservableCollection<Category<Book>>(temp);
            }
        }

    }
}
