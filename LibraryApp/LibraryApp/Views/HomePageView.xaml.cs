using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using LibraryApp.SubServiceLayer;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LibraryApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePageView : Page
    {
        private ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        private SubServiceLayer.SubServiceLayerClient service;
        public HomePageView()
        {
            this.InitializeComponent();
            service = new SubServiceLayerClient();
        }

        protected  override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            int userId = Convert.ToInt32(localSettings.Values["UserId"]);

            foreach (var book in await service.GetNewestBooksAsync(10))
            {
                ViewModel.NewestBooks.Add(book);
            }

            foreach (var book in await service.GetRecommendedBooksAsync(userId))
            {
                ViewModel.RecommendedBooks.Add(book);
            }
            
        }

        private void BookClick(object sender, ItemClickEventArgs e)
        {
            var frame = this.Parent;
            ((Frame)frame).Navigate(typeof(Views.BookView), e.ClickedItem);
        }
    }
}
