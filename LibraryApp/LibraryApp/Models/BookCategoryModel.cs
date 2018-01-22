using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using LibraryApp.SubServiceLayer;

namespace LibraryApp.Models
{
    public class BookCategoryModel
    {
        public string CategoryTitle { get; set; }
        public ObservableCollection<Book> Books { get; set; }
    }
}
