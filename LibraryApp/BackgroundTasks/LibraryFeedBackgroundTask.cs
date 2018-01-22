using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.Web.Syndication;

namespace BackgroundTasks
{
    public sealed class LibraryFeedBackgroundTask :IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {

            var x = Config.UserId;
            // Get a deferral, to prevent the task from closing prematurely 
            // while asynchronous code is still running.
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            // Download the feed.
            var feed = await GetTileFeed();

            var notificationFeed = await GetNotification();

            // Update the live tile with the feed items.
            UpdateTile(feed);
            ShowNotification(notificationFeed);

            // Inform the system that the task is finished.
            deferral.Complete();
        }

        private static async Task<XmlDocument> GetTileFeed()
        {
            XmlDocument feed = null;

            try
            {
                feed = await XmlDocument.LoadFromUriAsync(new Uri(Config.TileUpdateUlr));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return feed;
        }

        private static async Task<XmlDocument> GetNotification()
        {
            XmlDocument feed = null;

            try
            {
                feed = await XmlDocument.LoadFromUriAsync(new Uri(Config.NotificationUrl));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }

            return feed;
        } 

        private static void UpdateTile(XmlDocument feed)
        {
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(true);
            updater.Clear();

            updater.Update(new TileNotification(feed));           
        }

        private static void ShowNotification(XmlDocument feed)
        {
            var toastNotification = new ToastNotification(feed);
            var notification = ToastNotificationManager.CreateToastNotifier();
            notification.Show(toastNotification);
        }

        static string url =
            $@"http://localhost:8733/Design_Time_Addresses/ProjectLibraryService/TileService/UpdateTile/{Config.UserId}";

        static string notificationsUrl =
            @"http://localhost:8733/Design_Time_Addresses/ProjectLibraryService/TileService/GetNotification/1";

    }

}
