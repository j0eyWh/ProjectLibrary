using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ProjectLibraryService
{
    public class BooksReportData
    {

        public string Category { get; set; }
        public int CategoryTitlesCount { get; set; }
        public int CategoryAmount { get; set; }
    }
}
