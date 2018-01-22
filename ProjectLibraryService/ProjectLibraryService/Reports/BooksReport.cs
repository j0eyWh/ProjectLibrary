using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLibraryService
{
    public class BooksReport
    {
        public int TitlesCount { get; set; }
        public int TotalAmount { get; set; }

        public Dictionary<string , BooksReportData> CategoryInfo { get; set; }

        public BooksReport()
        {
            this.CategoryInfo = new Dictionary<string, BooksReportData>();
        }
    }
}
