using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLibraryService
{
    public class AnualReportData
    {
        public int Year { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalValue { get; set; }
        public int NrOfCollections { get; set; }
        public decimal AverageValue { get; set; }
    }
}
