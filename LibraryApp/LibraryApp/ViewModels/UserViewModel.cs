using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using LibraryApp.SubServiceLayer;

namespace LibraryApp.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged
    {
        SubServiceLayer.SubServiceLayerClient service = new SubServiceLayer.SubServiceLayerClient();

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        private  SubServiceLayer.SubServiceLayerClient client = new SubServiceLayer.SubServiceLayerClient();

        private User user = new User();
        public User User
        {
            get { return user; }
            set
            {
                user = value;
                var loadImageTask = LoadImageAsync();
                UserFullName = $"{User.Name} {User.Surname}";
                NotifyPropertyChanged("User");
                TotalBooksOnHands=service.GetUserBooksOnHandsCountAsync(User).Result;
            }
        }

        private string _userFullName;
        public string UserFullName
        {
            get { return _userFullName; }
            set { _userFullName = value; NotifyPropertyChanged(nameof(UserFullName)); }
        }


        private int _totalBooksOnHands;
        public int TotalBooksOnHands
        {
            get { return _totalBooksOnHands; }
            set { _totalBooksOnHands = value; NotifyPropertyChanged(nameof(TotalBooksOnHands)); }
        }


        private ImageSource imageSource;
        public ImageSource ImageSource
        {
            get { return imageSource; }
            set
            {
                imageSource = value;
                NotifyPropertyChanged("ImageSource");
            }
        }


        private async Task LoadImageAsync()
        {
            using (InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream())
            {
                // Writes the image byte array in an InMemoryRandomAccessStream
                // that is needed to set the source of BitmapImage.
                using (DataWriter writer = new DataWriter(ms.GetOutputStreamAt(0)))
                {
                    writer.WriteBytes(User.UserPic);
                    await writer.StoreAsync();
                }
                var image = new BitmapImage();
                await image.SetSourceAsync(ms);
                ImageSource = image;
            }
        }

      
    }
}
