using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryApp.SubServiceLayer;
using Book = LibraryApp.SubServiceLayer.Book;

namespace LibraryApp.ViewModels
{
    public class SampleViewModel
    {
        private ObservableCollection<Book> myVar;

        public ObservableCollection<Book> MyProperty
        {
            get { return myVar; }
            set { myVar = value; }
        }

        public SampleViewModel()
        {
            myVar = new ObservableCollection<Book>()
            {
                new Book {Author ="test 1", Title="test" }
            };
        }

    }
}
