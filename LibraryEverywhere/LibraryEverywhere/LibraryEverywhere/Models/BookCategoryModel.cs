using System;
using System.Collections.ObjectModel;

namespace LibraryEverywhere.Models
{
    public class BookCategoryModel:ObservableCollection<Book>
    {
        public string CategoryTitle { get; set; }
        public string ShortTitle { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
