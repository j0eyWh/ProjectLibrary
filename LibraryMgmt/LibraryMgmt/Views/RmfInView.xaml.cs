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

namespace LibraryMgmt.Views
{
    /// <summary>
    /// Interaction logic for RmfInView.xaml
    /// </summary>
    public partial class RmfInView : Page
    {
        public RmfInView()
        {
            InitializeComponent();
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //ViewModels.ListViewModel.SelectedInRecords.Clear();
            //if (listView.SelectedItems != null)
            //{
            //    foreach (var item in listView.SelectedItems)
            //    {
            //        ViewModels.ListViewModel.SelectedInRecords.Add((Rmf)item);

            //    }
            //}

        }

    }
}
