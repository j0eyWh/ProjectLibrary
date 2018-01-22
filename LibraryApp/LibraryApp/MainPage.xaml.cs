using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Windows.ApplicationModel.Background;
using Windows.Devices.Usb;
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
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.Web.Http;
using LibraryApp.SubServiceLayer;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LibraryApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        private TileUpdater _tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();

        private SubServiceLayerClient service = new SubServiceLayerClient();

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var userCode = localSettings.Values["UserCode"].ToString();
            
            this.RegisterBackgroundTask();
            RegisterNotificationsBackgroundTask();

            if (UserContext.CurrentUser == null)
                UserContext.CurrentUser = await service.GetUserAsync(localSettings.Values["UserCode"].ToString());

            //await RefreshTile();
        }

        #region Privates

        private async Task RefreshTile()
        {
            Windows.Data.Xml.Dom.XmlDocument xdoc = new Windows.Data.Xml.Dom.XmlDocument();
            
            HttpClient httpClient = new HttpClient();

           
            var httpResponse = await httpClient.SendRequestAsync(new HttpRequestMessage(HttpMethod.Get,
                new Uri(BackgroundTasks.Config.TileUpdateUlr)));

            xdoc.LoadXml(httpResponse.Content.ToString());

            _tileUpdater.Update(new TileNotification(xdoc));
        }


        //TODO: Decide which method for registering is better
        private async void RegisterBackgroundTask()
        {
            var taskName = "TileBackgroundTask";
            var taskEntryPoint ="BackgroundTasks.LibraryFeedBackgroundTask";

            var backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();
            if (backgroundAccessStatus == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity ||
                backgroundAccessStatus == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity)
            {
                foreach (var task in BackgroundTaskRegistration.AllTasks)
                {
                    if (task.Value.Name == taskName)
                    {
                        task.Value.Unregister(true);
                    }
                }

                BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder();
                taskBuilder.Name = taskName;
                taskBuilder.TaskEntryPoint = taskEntryPoint;
                taskBuilder.SetTrigger(new TimeTrigger(15, false));
                var registration = taskBuilder.Register();
            }
        }
        //TODO: Make this realy update notifications
        private async void RegisterNotificationsBackgroundTask()
        {
            var taskName = "Notify";
            var taskEntry = "BackgroundTasks.NotificationsBackgroundTask";

            var taskRegistered = false;

            foreach (var task in BackgroundTaskRegistration.AllTasks) 
            {
                if (task.Value.Name == taskName)
                {
                    taskRegistered = true;
                    break;
                }
            }

            if (!taskRegistered)
            {
                var taskBuilder = new BackgroundTaskBuilder();
                taskBuilder.Name = taskName;
                taskBuilder.TaskEntryPoint = taskEntry;
                taskBuilder.SetTrigger(new TimeTrigger(15, false));

                var registration = taskBuilder.Register();

            }
        }

        #endregion

        #region EventHandlers
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }


        private async void LogOffBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog m = new MessageDialog("Are you sure you want to log off? You'll have to reintroduce your credentials", "Are you sure you want to leave?");

            m.Commands.Add(new UICommand("Yes", CommandHandler));
            m.Commands.Add(new UICommand("No", CommandHandler));

            m.DefaultCommandIndex = 0;
            m.CancelCommandIndex = 1;
            await m.ShowAsync();
        }

        private void CommandHandler(IUICommand command)
        {
            switch (command.Label)
            {
                case "Yes":
                    {
                        Frame.Navigate(typeof(LogInView));
                        localSettings.Values["RememberUser"] = "false";
                        UserContext.CurrentUser = null;
                        _tileUpdater.Clear();
                        break;
                    }
                default:
                    break;
            }
        }

        private void AccountInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(Views.UserAccoutDetailsView));
        }

        private void HomeBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(Views.HomePageView));
        }

        private void ReadingBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(Views.BookOnHandsView));
        }

        private void IveReadButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(Views.BookPreviouslyOwnedView));
        }

        private void ReadingBtn_Click(object sender, TappedRoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(Views.BookOnHandsView));
        }

        private void IWillReadButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(Views.ReservedBooksView));
        }

        private void LibraryButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(Views.LibraryView));
        }
        #endregion
    }
}
