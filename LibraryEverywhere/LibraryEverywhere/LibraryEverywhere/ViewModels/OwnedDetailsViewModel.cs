using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryEverywhere.Common;
using LibraryEverywhere.Models;

namespace LibraryEverywhere.ViewModels
{
    public class OwnedDetailsViewModel:ObservableBase
    {
        private OnHand _ownedBook = new OnHand();

        public OnHand OwnedBook
        {
            get
            {
                return _ownedBook;
            }
            set
            {
                _ownedBook = value;
                NotifyPropertyChanged(nameof(OwnedBook));
            }
        }
    }
}
