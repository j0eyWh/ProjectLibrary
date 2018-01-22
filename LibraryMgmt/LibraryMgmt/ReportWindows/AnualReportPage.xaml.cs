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

namespace LibraryMgmt.ReportWindows
{
    /// <summary>
    /// Interaction logic for AnualReportPage.xaml
    /// </summary>
    public partial class AnualReportPage : Page
    {
        public LibraryMgmt.ServiceReference.LibraryServiceClient service { get; set; }

        public AnualReportPage()
        {
            InitializeComponent();

            service = new ServiceReference.LibraryServiceClient();
            foreach (var result in service.GenerateAnualReport().AboutEntries)
            {
                Label yearLbl = new Label();
                Label totalQuantityLbl = new Label();
                Label totalValueLbl = new Label();
                Label nrOfCollectionsLbl = new Label();
                Label averageValueLbl = new Label();

                yearLbl.Content = result.Key.ToString();
                totalQuantityLbl.Content = result.Value.TotalQuantity;
                totalValueLbl.Content = result.Value.TotalValue;
                nrOfCollectionsLbl.Content = result.Value.NrOfCollections;
                averageValueLbl.Content = result.Value.AverageValue;

                resultGrid.Children.Add(yearLbl);
                resultGrid.Children.Add(totalQuantityLbl);
                resultGrid.Children.Add(totalValueLbl);
                resultGrid.Children.Add(nrOfCollectionsLbl);
                resultGrid.Children.Add(averageValueLbl);
            }

            foreach (var result in service.GenerateAnualReport().AboutExits)
            {
                Label yearLbl = new Label();
                Label totalQuantityLbl = new Label();
                Label totalValueLbl = new Label();
                Label nrOfCollectionsLbl = new Label();
                Label averageValueLbl = new Label();

                yearLbl.Content = result.Key.ToString();
                totalQuantityLbl.Content = result.Value.TotalQuantity;
                totalValueLbl.Content = result.Value.TotalValue;
                nrOfCollectionsLbl.Content = result.Value.NrOfCollections;
                averageValueLbl.Content = result.Value.AverageValue;

                resultGrid2.Children.Add(yearLbl);
                resultGrid2.Children.Add(totalQuantityLbl);
                resultGrid2.Children.Add(totalValueLbl);
                resultGrid2.Children.Add(nrOfCollectionsLbl);
                resultGrid2.Children.Add(averageValueLbl);
            }

        }
    }
}
