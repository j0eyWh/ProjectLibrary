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
using Microsoft.Win32;
using System.IO;
using LibraryMgmt.Common;
using static LibraryMgmt.Common.ServiceClient;

namespace LibraryMgmt.MessageBoxes
{
    /// <summary>
    /// Interaction logic for EditBookWindow.xaml
    /// </summary>
    public partial class EditBookWindow : Window
    {
        private OpenFileDialog openDialog;
        

        public Book UpdatedBook { get; set; }
        public Book OldBook { get; set; }
        public EditBookWindow(Book book)
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

            CollectionCb.SelectedItem = ViewModelsGateway.RmfInViewModel.RmfInList
                .Single(x => x.IdRmf == book.CollectionId).IdRmf;

            titleTb.Text = book.Title;
            AuthorTb.Text = book.Author;
            PublisherTb.Text = book.Publisher;
            YearTb.Text = book.Year.ToString();
            PriceTb.Text = book.Price.ToString();
            CanBeLendCb.IsChecked = book.CanLend;
            DescriptionTb.Text = book.Description;
            AmountTb.Text = book.Amount.ToString();
            if (book.Image!=null)
            {
                MemoryStream stream = new MemoryStream(book.Image);
                image.Source = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad); 
            }

            OldBook = book;
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

                    int registeredQuantity = Client.GetAllBooks().Where(x => x.CollectionId == OldBook.CollectionId && x.BookId != OldBook.BookId).Sum(x => x.Amount);

                    if (registeredQuantity == 0)
                    {
                        int booksAlreadyRegisterd = Client.GetAllBooks().Where(x => x.BookId == OldBook.BookId).Sum(x => x.Amount);
                        if (booksAlreadyRegisterd == collectionQuantity)
                        {
                            registeredQuantity = collectionQuantity;
                        }
                    }

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

        private void CollectionCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (autoGenerateTb.IsChecked == true)
            {
                string collectionId = CollectionCb.SelectedItem as string;

                int collectionQuantity = ViewModelsGateway.RmfInViewModel
                            .RmfInList
                            .Single(x => x.IdRmf == collectionId)
                            .Quantity;

                int registeredQuantity = Client.GetAllBooks().Where(x => x.CollectionId == OldBook.CollectionId && x.BookId != OldBook.BookId).Sum(x => x.Amount);

                int availabe = collectionQuantity - registeredQuantity;

                AmountTb.Text = availabe.ToString();
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
            Book toBeIntroduced;
            try
            {
                //string c = ((ComboBoxItem)CategoryCb.SelectedItem).Content.ToString();

                var c = CategoryCb.Text;

                toBeIntroduced = new Book(
                        title: titleTb.Text,
                        author: AuthorTb.Text,
                        collectionId: (string)CollectionCb.SelectedItem,
                        regDate: DateTime.Now,
                        publisher: PublisherTb.Text,
                        year: Convert.ToInt16(YearTb.Text),
                        price: Convert.ToDecimal(PriceTb.Text),
                        lendable: (bool)CanBeLendCb.IsChecked,
                        description: DescriptionTb.Text,
                        amount: Convert.ToInt16(AmountTb.Text),
                        imgPath: openDialog.FileName,
                        category: c
                        );

                toBeIntroduced.BookId = OldBook.BookId;
                if (openDialog.FileName=="")
                {
                    toBeIntroduced.Image = OldBook.Image; 
                }

                if (toBeIntroduced.Amount < OldBook.Amount)
                {
                    if (Client.GetBookCodesOnHand(OldBook).Any())
                    {
                        MessageBox.Show("Sorry, you cant reduce the amount while there are books on hands");
                        return;
                        
                    }

                }

                Client.UpdateBook(toBeIntroduced);

                UpdatedBook = toBeIntroduced;
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
