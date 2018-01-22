using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLibraryService
{
    public class AnualReport
    {
        public Dictionary<int, AnualReportData> AboutEntries { get; set; }
        public Dictionary<int, AnualReportData> AboutExits { get; set; }

      
    }
}
