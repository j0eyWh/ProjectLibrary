using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LibraryApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReservedBooksView : Page
    {
        SubServiceLayer.SubServiceLayerClient service = new SubServiceLayer.SubServiceLayerClient();

        ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public ReservedBooksView()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ReservedBooksViewModel.Visibility = Windows.UI.Xaml.Visibility.Visible;
            base.OnNavigatedTo(e);
            int userId = Convert.ToInt32(localSettings.Values["UserId"]);

            foreach (var item in await service.GetUserReservedBooksAsync(userId))
            {
                ReservedBooksViewModel.ReservedBooks.Add(item);
            }

            
            ReservedBooksViewModel.Visibility= Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void GridView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var frame = this.Parent;
            ((Frame)frame).Navigate(typeof(Views.ReservedBookView), e.ClickedItem);
        }
    }
}
