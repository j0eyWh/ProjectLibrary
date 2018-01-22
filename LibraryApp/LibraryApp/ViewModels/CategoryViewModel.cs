using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using LibraryApp.SubServiceLayer;

namespace LibraryApp.ViewModels
{
    public class CategoryViewModel
    {
        private string _category;

        public string Category
        {
            get { return _category; }
            set { _category = value; }
        }


        private ObservableCollection<Book> _books = new ObservableCollection<Book>();

        public ObservableCollection<Book> Books
        {
            get { return _books; }
            set { _books = value; }
        }

    }
}
