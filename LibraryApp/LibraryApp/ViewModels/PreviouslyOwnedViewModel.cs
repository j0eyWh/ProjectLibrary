using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryApp.SubServiceLayer;

namespace LibraryApp.ViewModels
{
    public class PreviouslyOwnedViewModel
    {
        private ObservableCollection<BookCodeOwned> _previouslyOwnedBooks = new ObservableCollection<BookCodeOwned>();

        public ObservableCollection<BookCodeOwned> PreviouslyOwnedBooks
        {
            get { return _previouslyOwnedBooks; }
            set { _previouslyOwnedBooks = value; }
        }             

    }
}
