using System;
using System.Windows.Controls;
using LibraryMgmt.ServiceReference;
using System.Globalization;
using static LibraryMgmt.Common.ServiceClient;

namespace LibraryMgmt.ReportWindows
{
    /// <summary>
    /// Interaction logic for GeneralBookReportPage.xaml
    /// </summary>
    public partial class GeneralBookReportPage : Page
    {
        public GeneralBookReportPage()
        {
            InitializeComponent();
            DateTime now = DateTime.Now;
            CultureInfo culture = new CultureInfo("ro-Md");
            string date = now.ToString("D", culture);

            BooksReport report = Client.GenerateBookReport();

            
            signnatureLbl.Content = $"generat: {date}, ora: {DateTime.Now.ToShortTimeString()}";
            TitleCountLbl.Content = report.TitlesCount;
            TotalAmountLbl.Content = report.TotalAmount;

            foreach (var item in report.CategoryInfo)
            {
                Label categoryLbl = new Label();
                Label totalAmountLbl = new Label();
                Label titleCountLbl = new Label();

                categoryLbl.Content = item.Value.Category;
                totalAmountLbl.Content = item.Value.CategoryAmount;
                titleCountLbl.Content = item.Value.CategoryTitlesCount;

                unGrid.Children.Add(categoryLbl);
                unGrid.Children.Add(totalAmountLbl);
                unGrid.Children.Add(titleCountLbl);
            }

        }
    }
}
