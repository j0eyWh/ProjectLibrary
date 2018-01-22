using System;
using System.Collections.Generic;
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

namespace LibraryMgmt.ReportWindows
{
    /// <summary>
    /// Interaction logic for DetailsReportPage.xaml
    /// </summary>
    public partial class DetailsReportPage : Page
    {
        public DetailsReportPage(Rmf selectedRmf)
        {
            InitializeComponent();

            string s = $"{selectedRmf.IdRmf.Split('_')[0]} __ {selectedRmf.IdRmf.Split('_')[1]}";

            SubtitleLbl.Content = $"cu privire la inregistrarea cu numarul: {s}";

            dateInLbl.Content = selectedRmf.DateIn;
            docIdLbl.Content = selectedRmf.ContentCat;
            quantityLbl.Content = selectedRmf.Quantity;
            totalValueLbl.Content = selectedRmf.TotalValue;
            firstInvNrLbl.Content = selectedRmf.FirstInvNr;
            lastInvNrLbl.Content = selectedRmf.LastInvNr;
            contentLbl.Content = selectedRmf.ContentCat;
            originLbl.Content = selectedRmf.Origin;

            if (selectedRmf.IsOut == true)
            {
                outCauseLbl.Content = selectedRmf.OutCause;
            }
            else
            {
                outCauseLbl.Content = "nu este scoasa din evidenta";
            }


        }
    }
}
