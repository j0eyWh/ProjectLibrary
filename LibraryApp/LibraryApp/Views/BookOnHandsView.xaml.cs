using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LibraryApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BookOnHandsView : Page
    {
        SubServiceLayer.SubServiceLayerClient service = new SubServiceLayer.SubServiceLayerClient();

        ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public BookOnHandsView()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ProgressBar.Visibility = Visibility.Visible;
            int userId = Convert.ToInt32(localSettings.Values["UserId"]);

            foreach (var item in await service.GetUserBookAsync(userId))
            {
                ViewModel.OnHandsBooks.Add(item);
            }

            ProgressBar.Visibility = Visibility.Collapsed;
        }

        private void BookClick(object sender, ItemClickEventArgs e)
        {
            var frame = this.Parent;
            ((Frame)frame).Navigate(typeof(Views.OnHandBookView), e.ClickedItem);
        }
    }
}
