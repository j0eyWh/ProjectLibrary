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
using LibraryMgmt.ServiceReference;
using System.IO;

namespace LibraryMgmt.ReportWindows
{
    /// <summary>
    /// Interaction logic for DetailsReportWindow.xaml
    /// </summary>
    public partial class DetailsReportWindow : Window
    {
        public DetailsReportWindow(Rmf selectedRmf)
        {
            InitializeComponent();

            FixedDocument fixedDocument = new FixedDocument();
            PageContent pageContent = new PageContent();
            FixedPage fixedPage = new FixedPage();

            DetailsReportPage page = new DetailsReportPage(selectedRmf);
            DetailsReportImagePage pageImg = new DetailsReportImagePage(selectedRmf);

            Grid contentGrid = page.ContentGrid;
            page.MainGrid.Children.Remove(contentGrid);


            fixedPage.Children.Add(contentGrid);

            ((System.Windows.Markup.IAddChild)pageContent).AddChild(fixedPage);
            fixedDocument.Pages.Add(pageContent);

            PageContent pageWithImg = new PageContent();
            FixedPage fixedPageImg = new FixedPage();


            Grid imageGrid = pageImg.contentGrid;
            pageImg.MainGrid.Children.Remove(imageGrid);

            fixedPageImg.Children.Add(imageGrid);
            ((System.Windows.Markup.IAddChild)pageWithImg).AddChild(fixedPageImg);
            fixedDocument.Pages.Add(pageWithImg);

            documentViewr.Document = fixedDocument;

        }
    }
}
