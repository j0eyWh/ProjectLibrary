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
using System.IO;
using LibraryMgmt.ServiceReference;
using LibraryMgmt;
using GHM;
using static LibraryMgmt.Common.ServiceClient;

namespace LibraryMgmt.MessageBoxes
{
    /// <summary>
    /// Interaction logic for NewItemWindow.xaml
    /// </summary>
    public partial class NewItemWindow : Window
    {
        OpenFileDialog openDialog;

        public Rmf generatedRmf { get; set; }


        public NewItemWindow()
        {
            openDialog = new OpenFileDialog();
            openDialog.Filter = "Images (.jpg, .png, .bmp)|*.jpg;*.bmp;*.png";
            InitializeComponent();

            idRmfTb.Text = Hm.GenerateNewId(Client.GetLastIndex());
            dateInTb.Text = DateTime.Now.ToString();
            //lastInvNrTb.Text = "Introduceti primul numar de inventar";
            //textBox.DataContext = r;
        }

        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (openDialog.ShowDialog(this) == true)
            {
                FileStream f = new FileStream(openDialog.FileName, FileMode.Open, FileAccess.Read);
                image.Source = BitmapFrame.Create(f, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Sorry, not implemented. Yet...", "Not implemented :(", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = false;
            try
            {
                string contentCat;
                string origin;
                if (contentCatCombo.SelectedIndex == contentCatCombo.Items.Count-1)
                {
                    contentCat = ((TextBox)contentCatCombo.SelectedItem).Text;
                }
                else
                {
                    contentCat = ((ComboBoxItem)contentCatCombo.SelectedItem).Content.ToString();
                }

                if (originCombo.SelectedIndex == originCombo.Items.Count-1)
                {
                    origin = ((TextBox)originCombo.SelectedItem).Text;
                }
                else
                {
                    origin = ((ComboBoxItem)originCombo.SelectedItem).Content.ToString();
                }

                generatedRmf = new Rmf(
                    isOut: false,
                    idRmf: idRmfTb.Text,
                    dateIn: (DateTime?)Convert.ToDateTime(dateInTb.Text),
                    imgPath: openDialog.FileName,
                    docId: docInTb.Text,
                    quantity: Int32.Parse(quantityTb.Text),
                    totalValue: Convert.ToDecimal(totalValueTb.Text),
                    firstInvNumber: Convert.ToInt32(firstInvNr.Text),
                    contentCat: contentCat,
                    origin: origin,
                    dateOut: null,
                    outCause: null);
                isValid = true;
                
            }
            
            catch (ArgumentException exc)
            {
                MessageBox.Show(exc.Message);
            }

            catch (Exception eex)
            {
                MessageBox.Show(eex.Message);
            }

            if (isValid == true)
            {
                DialogResult = true;
            }
        }

        private void firstInvNr_TextChanged(object sender, TextChangedEventArgs e)
        {
            int x, q;

            if (int.TryParse(firstInvNr.Text, out x) && int.TryParse(quantityTb.Text, out q))
            {
                lastInvNrTb.Text = Hm.GenerateLastInvNr(x, q).ToString();
            }
        }

   

        private void autoGenerateCb_Checked(object sender, RoutedEventArgs e)
        {
            if (autoGenerateCb.IsChecked == true)
            {
                firstInvNr.IsEnabled = false;

                firstInvNr.Text = (Client.GetLastLastInvNr() + 1).ToString();
            }
            else
            {
                firstInvNr.IsEnabled = true;
            }
        }
    }
}
