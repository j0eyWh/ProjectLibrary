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
    public class ReservationsViewModel:ObservableBase
    {
        private ObservableCollection<Category<ReservedBook>> _reservedBooks = new ObservableCollection<Category<ReservedBook>>();

        public ObservableCollection<Category<ReservedBook>> ReservedBooks
        {
            get { return _reservedBooks; }
            set { _reservedBooks = value; NotifyPropertyChanged(nameof(ReservedBooks));}
        }

        public async Task RefreshViewModel()
        {
            var r = await Client.CurrentInstance.GetAsync($"users/user/{UserContext.CurrentUser.UserId}/reserved");

            if (r.IsSuccessStatusCode)
            {
                var reservedBooks = await r.Content.ReadAsAsync<IEnumerable<ReservedBook>>();

                var groupedReservedBooks = reservedBooks.GroupBy(x=>x.BookCode.Book.Title[0]);

                var collection = new ObservableCollection<Category<ReservedBook>>();

                foreach (var groupedReservedBook in groupedReservedBooks)
                {
                    var c = new Category<ReservedBook>()
                    {
                        Name = groupedReservedBook.Key.ToString(),
                        ShortName = groupedReservedBook.Key.ToString()
                    };

                    foreach (var book in groupedReservedBook)
                    {
                        c.Add(book);
                    }

                    collection.Add(c);
                }

                ReservedBooks = new ObservableCollection<Category<ReservedBook>>(collection.OrderBy(x=>x.Name));
            }
        }

    }
}
