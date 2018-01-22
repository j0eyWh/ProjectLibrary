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
    /// Interaction logic for AnualReportWindow.xaml
    /// </summary>
    public partial class AnualReportWindow : Window
    {
        public AnualReportWindow()
        {
            InitializeComponent();

            AnualReportPage page = new AnualReportPage();
            Grid ContentGrid = page.ContentGrid;
            page.MainGrid.Children.Remove(ContentGrid);

            FixedDocument fixedDoc = new FixedDocument();
            PageContent pageContent = new PageContent();
            FixedPage fixedPage = new FixedPage();

            fixedPage.Children.Add(ContentGrid);
            ((System.Windows.Markup.IAddChild)pageContent).AddChild(fixedPage);
            fixedDoc.Pages.Add(pageContent);

            documentViewer.Document = fixedDoc;
            
        }
    }
}
