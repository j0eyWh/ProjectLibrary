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
    public class FollowedCategoriesViewModel : ObservableBase
    {
        private ObservableCollection<Category<Book>> _books = new ObservableCollection<Category<Book>>();

        public ObservableCollection<Category<Book>> Books
        {
            get { return _books; }
            set { _books = value; NotifyPropertyChanged(nameof(Books)); }
        }

        

        public ObservableCollection<Book> RecentBooks { get; set; } = new ObservableCollection<Book>();


        public async Task RefreshViewModel()
        {
            var c = await Client.CurrentInstance.GetAsync($"users/{UserContext.CurrentUser.UserId}/following");

            if (c.IsSuccessStatusCode)
            {
                var groupedBooks = (await c.Content.ReadAsAsync<IEnumerable<Book>>()).GroupBy(x => x.Category).OrderBy(x=>x.Key);

                List<Category<Book>> categories = new List<Category<Book>>();

                foreach (var bookGroup in groupedBooks)
                {
                    RecentBooks.Add(bookGroup.OrderByDescending(x=>x.RegistrationDate).FirstOrDefault());

                    var category = new Category<Book>()
                    {
                        Name = bookGroup.Key,
                        ShortName = bookGroup.Key.Substring(0, 2)
                    };

                    foreach (var book in bookGroup)
                    {
                        category.Add(book);
                    }

                    Books.Add(category);

                }

            }


        }
    }
}
