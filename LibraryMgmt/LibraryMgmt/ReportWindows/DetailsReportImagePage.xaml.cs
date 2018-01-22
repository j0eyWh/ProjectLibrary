using LibraryMgmt.ServiceReference;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace LibraryMgmt.ReportWindows
{
    /// <summary>
    /// Interaction logic for DetailsReportImagePage.xaml
    /// </summary>
    public partial class DetailsReportImagePage : Page
    {
        public DetailsReportImagePage(Rmf selectedRmf)
        {
            InitializeComponent();

            if (selectedRmf.DocImg!=null)
            {
                MemoryStream stream = new MemoryStream(selectedRmf.DocImg);
                docImg.Source = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }
        }
    }
}
