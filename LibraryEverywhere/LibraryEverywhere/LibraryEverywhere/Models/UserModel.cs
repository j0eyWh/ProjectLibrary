using System;
using System.IO;
using System.Runtime.Serialization;
using LibraryEverywhere.Common;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace LibraryEverywhere.Models
{
    public class User :ObservableBase
    {
        public int UserId { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Idnp { get; set; }

        public byte[] UserPic { get; set; }

        public DateTime BirthDate { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

      
        private ImageSource _imageSource = null;
        public ImageSource ImageSource
        {
            get
            {
                if (_imageSource != null) return _imageSource;
                if (UserPic != null)
                {
                    _imageSource = ImageSource.FromStream(() => new MemoryStream(this.UserPic));
                }
                return _imageSource;
            }
            set { _imageSource = value; NotifyPropertyChanged(nameof(ImageSource)); }
        }

    }
}
