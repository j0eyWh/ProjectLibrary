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
    public class SpotlightViewModel : ObservableBase
    {

        public ObservableCollection<Book> SuggestedBooks { get; set; } = new ObservableCollection<Book>();

        public ObservableCollection<Book> NewestBooks
        {
            get; set;
        } = new ObservableCollection<Book>();

        public async Task RefreshViewModel()
        {
            var books = await GetNewestBooks();

            if (books != null)
            {
                foreach (var newestBook in books)
                {
                    NewestBooks.Add(newestBook);
                }
            }

            var suggestedBooks = await GetRecommendedBooks();
            if (suggestedBooks!=null)
            {
                foreach (var suggestedBook in suggestedBooks)
                {
                    SuggestedBooks.Add(suggestedBook);
                }
            }
        }

        public async Task<IEnumerable<Book>> GetNewestBooks()
        {
            var r = await Client.CurrentInstance.GetAsync("books/latest/5");

            if (r.IsSuccessStatusCode)
            {
                return await r.Content.ReadAsAsync<IEnumerable<Book>>();
            }
            else return null;
        }

        private async Task<IEnumerable<Book>> GetRecommendedBooks()
        {
            var r = await Client.CurrentInstance.GetAsync($"books/recommended/{UserContext.CurrentUser.UserId}");

            if (r.IsSuccessStatusCode)
            {
                return await r.Content.ReadAsAsync<IEnumerable<Book>>();
            }
            return null;
        }

        private int _newestPosition;

        public int NewestPosition
        {
            get { return _newestPosition; }
            set { _newestPosition = value; NotifyPropertyChanged(nameof(NewestPosition)); }
        }

        private int _suggestedPosition;

        public int SuggestedPosition
        {
            get { return _suggestedPosition; }
            set { _suggestedPosition = value; NotifyPropertyChanged(nameof(SuggestedPosition)); }
        }


    }
}
