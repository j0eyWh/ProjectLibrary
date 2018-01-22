using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLibraryService.Reports
{
    public class Part3ReportData
    {
        public int Year { get; set; }
        public int RegisteredBooks { get; set; }
        public int OutBooks { get; set; }

        public int BooksLeft { get { return RegisteredBooks - OutBooks; }
            set { } }
    }
}
