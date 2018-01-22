using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace ProjectLibraryService
{
    public partial class Book
    {
        
        public int BookId { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public DateTime RegistrationDate { get; set; }

        public int Amount { get; set; }

        [DefaultValue("s.n.")]
        public string Publisher { get; set; }

        /// <summary>
        /// when the book was published
        /// </summary>
        public int Year { get; set; }

        public decimal Price { get; set; }

        public bool CanLend { get; set; }

        public string Description { get; set; }

        public byte[] Image { get; set; }

        public string CollectionId { get; set; }

        [ForeignKey("CollectionId")]
        public Rmf Collection { get; set; }

        [DefaultValue("none")]
        public string Category { get; set; }

    }
}
