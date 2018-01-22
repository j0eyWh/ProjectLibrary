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
using GHM;
using System.IO;
using static LibraryMgmt.Common.ServiceClient;
namespace LibraryMgmt.MessageBoxes
{
    /// <summary>
    /// Interaction logic for BookCheckOutWindow.xaml
    /// </summary>
    public partial class BookCheckOutWindow : Window
    {
        public ReservedBook CheckedBook { get; set; }

        public BookCheckOutWindow(ReservedBook book)
        {
            InitializeComponent();
            BookLabel.Content = book.BookCode.Book.Title;
            UserLabel.Content = book.User.Name + " " + book.User.Surname;
            CodeLabel.Content = book.BookCode.Code;
            BookImage.Source = BitmapFrame.Create(new MemoryStream(book.BookCode.Book.Image), BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            CheckedBook = book;

        }

        private async void CheckOutButton_Click(object sender, RoutedEventArgs e)
        {
            if (DatePicker.SelectedDate==null || DatePicker.SelectedDate<=DateTime.Now)
            {
                MessageBox.Show("Selected return date must be in the future", "Wrong return date", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            using (Client)
            {
                await Client.AddRegAsync(new OnHand()
                {
                    Code = CheckedBook.Code,
                    LendDate = DateTime.Now,
                    UserId = CheckedBook.UserId,
                    ReturnDate = DatePicker.SelectedDate.Value

                });

                await Client.DeleteBookReservationAsync(CheckedBook.ReservationId);
            }
            DialogResult = true;
            this.Close();
        }
    }
}
