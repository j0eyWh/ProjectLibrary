using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Notifications;

namespace BackgroundTasks
{
    public static class Config
    {
        static ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        #region Configurations
        //Url for tile updating service
        private static string _tileUpdateUrl =
            $@"http://localhost:8733/Design_Time_Addresses/ProjectLibraryService/TileService/UpdateTile/{UserId}";


        //Url for notification provider service
        private static string _notificationsUrl =
            $@"http://localhost:8733/Design_Time_Addresses/ProjectLibraryService/TileService/GetNotification/{UserId}";
        #endregion

        #region Publics

        
        public static int UserId {
            get { return Convert.ToInt32(localSettings.Values["UserId"]); }
        }

        public static string TileUpdateUlr
        {
            get { return _tileUpdateUrl; }
        }

        public static string NotificationUrl {
            get { return _notificationsUrl; }
        }
        
        #endregion
    }
}
