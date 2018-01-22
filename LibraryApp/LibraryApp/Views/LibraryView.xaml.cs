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
using LibraryApp.Models;
using LibraryApp.SubServiceLayer;
using LibraryApp.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LibraryApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LibraryView : Page
    {
        private readonly SubServiceLayer.SubServiceLayerClient _service = new SubServiceLayer.SubServiceLayerClient();
        //public LibraryViewModel MainModel { get; set; }

        public LibraryView()
        {
            this.InitializeComponent();
            LibraryViewModel = new LibraryViewModel();
            //DataContext = MainModel;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var b = await _service.GetBooksCategoriesAsync();
            this.ProgressBar.Visibility = Visibility.Visible;

            var booksByCategories = b.GroupBy(x => x.Category).Select(s => new BookCategoryModel
            {
                CategoryTitle = s.Key,
                Books = new System.Collections.ObjectModel.ObservableCollection<Book>(s.ToList())
            });

            foreach (var item in booksByCategories)
            {
                LibraryViewModel.BooksByCategory.Add(item);
            }

            this.ProgressBar.Visibility = Visibility.Collapsed;

            var collectionGroups = BooksGroupsSource.View.CollectionGroups;
            ((ListViewBase) this.SemanticZoom.ZoomedOutView).ItemsSource = collectionGroups;
        }

     

        private void ToggleSemanticZoom(object sender, RoutedEventArgs e)
        {
            SemanticZoom.ToggleActiveView();

        }

        private void RedirectToCategoryPage(object sender, RoutedEventArgs e)
        {
            var categoryTitle = ((TextBlock)((StackPanel)  ((Button) sender).Parent).Children[0]).Text;
            var frame = this.Parent;
            ((Frame) frame).Navigate(typeof (Views.CategoryView), categoryTitle);
            return;
        }

        private void BookClick(object sender, ItemClickEventArgs e)
        {
            var frame = this.Parent;
            ((Frame)frame).Navigate(typeof(Views.BookView), e.ClickedItem);
        }

        private void SearchBtnClick(object sender, RoutedEventArgs e)
        {
            var frame = this.Parent;
            ((Frame)frame).Navigate(typeof(Views.BookSearchView));
        }
    }
}
