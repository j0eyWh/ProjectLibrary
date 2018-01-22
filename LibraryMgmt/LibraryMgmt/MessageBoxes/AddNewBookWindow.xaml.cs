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
using LibraryMgmt;
using Microsoft.Win32;
using System.IO;
using System.ServiceModel;
using LibraryMgmt.Common;
using static LibraryMgmt.Common.ServiceClient;
namespace LibraryMgmt.MessageBoxes
{
    /// <summary>
    /// Interaction logic for AddNewBookWindow.xaml
    /// </summary>
    public partial class AddNewBookWindow : Window
    {
        OpenFileDialog openDialog;

        public Book NewBook { get; set; }

        public AddNewBookWindow()
        {
            InitializeComponent();

            

            openDialog = new OpenFileDialog();
            openDialog.Filter = "Images (.jpg, .png, .bmp)|*.jpg;*.bmp;*.png";

            var s = ViewModelsGateway.RmfInViewModel.RmfInList
                .GroupBy(x => x.IdRmf)
                .Select(
                    x => new
                    {
                        year = x.FirstOrDefault().IdRmf
                    }
                );
                

            foreach (var item in s)
            {
                CollectionCb.Items.Add(item.year);
            }
        }

        private void autoGenerateTb_Click(object sender, RoutedEventArgs e)
        {
            string collectionId = CollectionCb.SelectedItem as string;

            if (collectionId != null)
            {
                if (((CheckBox)sender).IsChecked == true)
                {
                    AmountTb.IsEnabled = false;

                    int collectionQuantity = ViewModelsGateway.RmfInViewModel
                        .RmfInList
                        .Single(x => x.IdRmf == collectionId)
                        .Quantity;

                    int registeredQuantity = Client.GetBooksByRmfId(collectionId)
                        .Sum(x => x.Amount);

                    int availabe = collectionQuantity - registeredQuantity;

                    AmountTb.Text = availabe.ToString();

                }
                else
                {
                    AmountTb.IsEnabled = true;
                } 
            }
            else
            {
                MessageBox.Show("Select a Collection ID first");
                autoGenerateTb.IsChecked = false;
            }
        }

        private void openImgBtn_Click(object sender, RoutedEventArgs e)
        {
            if (openDialog.ShowDialog(this) == true)
            {
                FileStream f = new FileStream(openDialog.FileName, FileMode.Open, FileAccess.Read);
                image.Source = BitmapFrame.Create(f, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            };

          
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var toBeIntroduced = new Book(
                    title: titleTb.Text,
                    author: AuthorTb.Text,
                    collectionId: ((string)CollectionCb.SelectedItem),
                    regDate: DateTime.Now,
                    publisher: PublisherTb.Text,
                    year: Convert.ToInt32(YearTb.Text),
                    price: Convert.ToDecimal(PriceTb.Text),
                    lendable: (bool)CanBeLendCb.IsChecked,
                    description: DescriptionTb.Text,
                    amount: Convert.ToInt32(AmountTb.Text),
                    imgPath: openDialog.FileName,
                    category: ((ComboBoxItem)CategoryCb.SelectedItem).Content.ToString()
                    );

                if (Client.GetAllBooks().Any())
                {
                    Client.AddBook(toBeIntroduced);
                    toBeIntroduced.BookId = Client.GetAllBooks().OrderBy(x => x.BookId).Last().BookId;
                }
                else
                {
                    Client.AddBook(toBeIntroduced);
                    toBeIntroduced.BookId = Client.GetAllBooks().First().BookId;
                }
                NewBook = toBeIntroduced;
                DialogResult = true;
            }

            catch(ArgumentException argEx)
            {
                MessageBox.Show(argEx.Message);
            }
            catch (FaultException argE)
            {
                MessageBox.Show(argE.Message);
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.GetType().ToString()+"  "+ex.Message);
            }
            
            
        }

        private void CollectionCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (autoGenerateTb.IsChecked==true)
            {
                string collectionId = CollectionCb.SelectedItem as string;

                int collectionQuantity = ViewModelsGateway.RmfInViewModel
                            .RmfInList
                            .Single(x => x.IdRmf == collectionId)
                            .Quantity;

                int registeredQuantity = Client.GetBooksByRmfId(collectionId)
                    .Sum(x => x.Amount);

                int availabe = collectionQuantity - registeredQuantity;

                AmountTb.Text = availabe.ToString(); 
            }
        }
    }
}
