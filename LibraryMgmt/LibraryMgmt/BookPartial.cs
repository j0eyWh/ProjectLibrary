using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHM;

namespace LibraryMgmt.ServiceReference
{
    public partial class  Book
    {
        public Book(string title, string author, string collectionId, DateTime regDate, string publisher, int year, decimal price, bool lendable, string description, int amount, string imgPath, string category)
        {
            if (String.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Please provide a valid title");

            if (String.IsNullOrWhiteSpace(author))
                throw new ArgumentException("Please provide a valid author name");

            if (!GHM.Hm.IsValidIdRmf(collectionId))
                throw new ArgumentException($"Empty collection id or incorrect input format: {collectionId}");

            if (String.IsNullOrWhiteSpace(publisher))
                throw new ArgumentException("Empty publisher name is not allowed");

            if (year<1900 || year>DateTime.Now.Year)
                throw new ArgumentException($"Invalid year of publishment: {year}");

            if (price<0)
                throw new ArgumentException($"Price can't be {price}");

            if (amount<=0)
                throw new ArgumentException($"Amount can't be {amount}");

            if (String.IsNullOrWhiteSpace(category))
            {
                throw new ArgumentException(message: "Category cant be empty or null");
            }

            if (!String.IsNullOrWhiteSpace(imgPath))
            {
                this.Image = Hm.ImageToByteArray(imgPath);
            }
            else
            {
                this.Image = null;
            }

            this.Title = title;
            this.Author = author;
            this.CollectionId = collectionId;
            this.RegistrationDate = regDate;
            this.Publisher = publisher;
            this.Year = year;
            this.Price = price;
            this.CanLend = lendable;
            this.Description = description;
            this.Amount = amount;
            this.Category = category;


        }

    }
}
