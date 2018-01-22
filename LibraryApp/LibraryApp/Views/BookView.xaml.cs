using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
//using LibraryApp.SubService;
using LibraryApp.SubServiceLayer;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LibraryApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BookView : Page
    {
        private SubServiceLayerClient client = new SubServiceLayerClient();
        ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        
        public BookView()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ProgressBar.Visibility = Visibility.Visible;
            base.OnNavigatedTo(e);

            var book = e.Parameter as Book;

            if (book!=null)
            {
                BookViewModel.Book = book;
                BookViewModel.AvailableCount = await client.GetAvailableCountAsync(book.BookId);
            }

            ProgressBar.Visibility = Visibility.Collapsed;
        }

        private async void ReserveBook(object sender, RoutedEventArgs e)
        {
            ProgressBar.Visibility=Visibility.Visible;
            int userId = Convert.ToInt32(localSettings.Values["UserId"]);
            int bookId = BookViewModel.Book.BookId;
            var service = new SubServiceLayerClient();

            if (BookViewModel.AvailableCount<=0)
            {
                MessageDialog dialog = new MessageDialog("There are no books left :( ! Try again later.");
                await dialog.ShowAsync();
                return;
            }
            

            if (await service.HasReservedAsync(userId,bookId))
            {
                MessageDialog dialog = new MessageDialog("You cant reserve a book that you already reserved!");
                await dialog.ShowAsync();
                return;
            }

            if (await service.HasOnHandsAsync(userId, bookId))
            {
                MessageDialog dialog = new MessageDialog("You cant reserve a book that you already own!");
                await dialog.ShowAsync();
                return;
            }

            await service.AddReservationAsync(userId, bookId);

            MessageDialog d = new MessageDialog("You just reserved this book! You must take it within 3 days!");
            await d.ShowAsync();

            ProgressBar.Visibility = Visibility.Collapsed;
        }
    }
}
