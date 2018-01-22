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
using Microsoft.Win32;
using LibraryMgmt.ServiceReference;
using System.IO;
using GHM;

namespace LibraryMgmt.MessageBoxes
{
    /// <summary>
    /// Interaction logic for ImportFromScratchWindow.xaml
    /// </summary>
    public partial class ImportFromScratchWindow : Window
    {
        OpenFileDialog openDialog;
        public Rmf generatedRmf { get; set; }
        ServiceReference.LibraryServiceClient service = new ServiceReference.LibraryServiceClient();

        public ImportFromScratchWindow()
        {
            InitializeComponent();
            openDialog = new OpenFileDialog();
        }

        private void openBtn_Click(object sender, RoutedEventArgs e)
        {

            if (openDialog.ShowDialog(this) == true)
            {
                FileStream f = new FileStream(openDialog.FileName, FileMode.Open, FileAccess.Read);
                image.Source = BitmapFrame.Create(f, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = false;
            DateTime outDate;
            DateTime? outDateN;
            if (!DateTime.TryParse(dateOutTb.Text, out outDate))
            {
                outDateN = null;
            }
            else { outDateN = outDate; }

            try
            {
                generatedRmf = new Rmf(
                    isOut: (bool)isOutCb.IsChecked,
                    idRmf: idRmfTb.Text,
                    dateIn: Convert.ToDateTime(dateInTb.Text),
                    imgPath: openDialog.FileName,
                    docId: docInTb.Text,
                    quantity: Convert.ToInt32(quantityTb.Text),
                    totalValue: Convert.ToDecimal(totalValueTb.Text),
                    firstInvNumber: Convert.ToInt32(firstInvNrTb.Text),
                    contentCat: contentCatTb.Text,
                    origin: originTb.Text,
                    dateOut: outDateN,
                    outCause: outCouseTb.Text
                    );
                isValid = true;

            }
            catch(ArgumentException argE)
            {
                MessageBox.Show(argE.Message);
            }
            catch (Exception eE)
            {
                MessageBox.Show(eE.Message);
            }

            if (isValid)
            {
                DialogResult = true;
            }
        }

        private void firstInvNrTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            int x, q;
            if (int.TryParse(firstInvNrTb.Text, out x) && int.TryParse(quantityTb.Text, out q))
            {
                lastInvNrTb.Text = Hm.GenerateLastInvNr(x, q).ToString();
            }
        }
    }
}
