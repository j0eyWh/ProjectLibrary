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
using LibraryMgmt.Common;

namespace LibraryMgmt.ReportWindows
{
    /// <summary>
    /// Interaction logic for GeneralInReportWindow.xaml
    /// </summary>
    public partial class GeneralInReportWindow : Window
    {
        public GeneralInReportWindow()
        {
            InitializeComponent();

            
            GeneralReportPage page = new GeneralReportPage(ViewModelsGateway.RmfInViewModel.RmfInList, ViewModelsGateway.RmfOutViewModel.List);
            
            FixedDocument fixedDoc = new FixedDocument();
            PageContent pageContent = new PageContent();
            FixedPage fixedPage = new FixedPage();

            Canvas c = page.mainCanvas;
            page.MainGrid.Children.Remove(c);
            
            fixedPage.Children.Add(c);
            ((System.Windows.Markup.IAddChild)pageContent).AddChild(fixedPage);
            fixedDoc.Pages.Add(pageContent);

            documentViewer.Document = fixedDoc;

        }
    }
}
