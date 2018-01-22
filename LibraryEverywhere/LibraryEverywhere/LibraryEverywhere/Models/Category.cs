using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEverywhere.Models
{
    public class Category<T>:ObservableCollection<T>
    {
        public string Name { get; set; }
        public string ShortName { get; set; }

    }
}
