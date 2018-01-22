using System.Runtime.Serialization;

namespace DataGateway.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Book
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Book()
        {
            BookCodes = new HashSet<BookCode>();
        }

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

        [StringLength(50)]
        public string CollectionId { get; set; }

        public string Category { get; set; }

        [IgnoreDataMember]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookCode> BookCodes { get; set; }

        public virtual Rmf Rmf { get; set; }

        [NotMapped]
        public string ImageUrl { get { return $"http://10.102.0.198:35090/api/books/book/{BookId}/coverImage"; } }

        public virtual ICollection<BookTag> BookTags  { get; set; }

    }
}
