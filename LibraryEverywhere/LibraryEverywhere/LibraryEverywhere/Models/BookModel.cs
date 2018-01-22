using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

namespace LibraryEverywhere.Models
{
    public class Book
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]

        public int BookId { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public DateTime RegistrationDate { get; set; }

        public int Amount { get; set; }

        public string Publisher { get; set; }

        public int Year { get; set; }

        public decimal Price { get; set; }

        public bool CanLend { get; set; }

        public string Description { get; set; }

        public byte[] Image { get; set; }

        public string CollectionId { get; set; }

        public string Category { get; set; }

        public string ImageUrl { get; set; }

        public ImageSource CoverImage
        {
            get
            {
                return Image != null ? ImageSource.FromStream(() => new MemoryStream(Image)) : null;
            }
        }
        //public BitmapImage CoverImage
        //{
        //    get
        //    {
        //        if (this.Image.Length > 0)
        //        {
        //            return ConvertByteArrayToImage(this.Image);
        //        }
        //        else return null;

        //    }

        //}

        //private BitmapImage ConvertByteArrayToImage(byte[] buffer)
        //{
        //    using (InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream())
        //    {
        //        // Writes the image byte array in an InMemoryRandomAccessStream
        //        // that is needed to set the source of BitmapImage.
        //        using (DataWriter writer = new DataWriter(ms.GetOutputStreamAt(0)))
        //        {
        //            writer.WriteBytes((byte[])buffer);

        //            // The GetResults here forces to wait until the operation completes
        //            // (i.e., it is executed synchronously), so this call can block the UI.
        //            writer.StoreAsync().GetResults();
        //        }

        //        var image = new BitmapImage();
        //        image.SetSource(ms);
        //        return image;
        //    }

        //}
    }
}
