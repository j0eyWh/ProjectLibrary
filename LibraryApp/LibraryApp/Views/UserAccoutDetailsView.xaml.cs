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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LibraryApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserAccoutDetailsView : Page
    {

        private ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        private SubServiceLayer.SubServiceLayerClient client = new SubServiceLayer.SubServiceLayerClient();
        public UserAccoutDetailsView()
        {
            this.InitializeComponent();
        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (UserViewModel.User.Idnp == null)
            {
                UserViewModel.User = await client.GetUserAsync(localSettings.Values["UserCode"] as string);
            }
        }
    }
}
