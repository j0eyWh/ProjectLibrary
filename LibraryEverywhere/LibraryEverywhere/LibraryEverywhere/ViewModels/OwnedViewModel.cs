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
    public class OwnedViewModel:ObservableBase
    {
        private ObservableCollection<Category<OnHand>> _ownedBooks = new ObservableCollection<Category<OnHand>>();

        public ObservableCollection<Category<OnHand>> OwnedBooks
        {
            get { return _ownedBooks; }
            set { _ownedBooks = value; NotifyPropertyChanged(nameof(OwnedBooks)); }
        }

        public async Task RefreshViewModel()
        {
            OwnedBooks.Clear();
            var r = await Client.CurrentInstance.GetAsync($"users/user/{UserContext.CurrentUser.UserId}/onhands");

            if (r.IsSuccessStatusCode)
            {
                var onHandsBooks = await r.Content.ReadAsAsync<IEnumerable<OnHand>>();

                var groupedBooks = onHandsBooks.GroupBy(x => x.BookCode.Book.Title[0]).OrderBy(x=>x.Key);

                foreach (var bookGroup in groupedBooks)
                {
                    var category = new Category<OnHand>();

                    category.Name = bookGroup.Key.ToString();
                    category.ShortName = category.Name;

                    foreach (var book in bookGroup)
                    {
                        category.Add(book);
                    }

                    OwnedBooks.Add(category);
                }
            }
        }

    }
}
