using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LibraryMgmt.ServiceReference;
using LibraryMgmt.ViewModels;

namespace LibraryMgmt.ReportWindows
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class GeneralReportPage : Page
    {
        public GeneralReportPage(IEnumerable<Rmf>rmfInList, IEnumerable<Rmf> rmfOutList )
        {
            InitializeComponent();
            DateTime now = DateTime.Now;
            CultureInfo culture = new CultureInfo("ro-Md");
            string date = now.ToString("D", culture);

            //Calculating values
            int totalRmf = rmfInList.Count() + rmfOutList.Count();
            int totalBooks = rmfInList.Sum(x => x.Quantity) + rmfOutList.Sum(x => x.Quantity);
            int totalActiveRmf = rmfInList.Count();
            int totalActiveBooks = rmfInList.Sum(x => x.Quantity);
            int totalOutRmf = rmfOutList.Count();
            int totalOutBooks = rmfOutList.Sum(x => x.Quantity);
            decimal? totalRmfValue = rmfInList.Sum(x => x.TotalValue) + rmfOutList.Sum(x => x.TotalValue);
            decimal? averageRmfValue = (rmfInList.Average(x => x.TotalValue) + rmfOutList.Average(x => x.TotalValue)) / 2;
            label1.Content = $"generat: {date}, ora: {DateTime.Now.ToShortTimeString()} ";

            //Populating result labels:
            totalRmfLbl.Content = totalRmf.ToString();
            totalBooksLbl.Content = totalBooks.ToString();
            totalActiveRmfLbl.Content = totalActiveRmf.ToString();
            totalActiveBooksLbl.Content = totalActiveBooks.ToString();
            totalOutRmfLbl.Content = totalOutRmf.ToString();
            totalOutBooksLbl.Content = totalOutBooks.ToString();
            totalRmfValueLbl.Content = Convert.ToDecimal(totalRmfValue).ToString("c", culture);
            averageRmfValueLbl.Content = Convert.ToDecimal(averageRmfValue).ToString("c", culture);

        }
    }
}
