﻿using System;
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


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LibraryApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CategoryView : Page
    {

        private SubServiceLayer.SubServiceLayerClient service =new SubServiceLayer.SubServiceLayerClient();

        public CategoryView()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var b = await service.GetUserBookAsync(1);
            ProgressBar.Visibility = Visibility.Visible;
            string category = e.Parameter as string;
            CategoryViewModel.Category = category;
            foreach (var book in await service.GetBooksByCategoryAsync(category))
            {
                CategoryViewModel.Books.Add(book);
            }

            ProgressBar.Visibility = Visibility.Collapsed;


        }

        private void BookClick(object sender, ItemClickEventArgs e)
        {
            var frame = this.Parent;
            ((Frame)frame).Navigate(typeof(Views.BookView), e.ClickedItem);
        }
    }
}
