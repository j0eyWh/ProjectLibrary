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
using System.Windows.Shapes;

namespace LibraryMgmt.ReportWindows
{
    /// <summary>
    /// Interaction logic for GeneralBooksReport.xaml
    /// </summary>
    public partial class GeneralBooksReport : Window
    {
        public GeneralBooksReport()
        {
            InitializeComponent();

            GeneralBookReportPage page = new GeneralBookReportPage();

            Grid contentGrid = page.contentGrid;
            page.mainGrid.Children.Remove(contentGrid);

            FixedDocument fixedDoc = new FixedDocument();
            PageContent pageContent = new PageContent();
            FixedPage fixedPage = new FixedPage();

            fixedPage.Children.Add(contentGrid);
            ((System.Windows.Markup.IAddChild)pageContent).AddChild(fixedPage);
            fixedDoc.Pages.Add(pageContent);

            documentViewer.Document = fixedDoc;
        }
    }
}
