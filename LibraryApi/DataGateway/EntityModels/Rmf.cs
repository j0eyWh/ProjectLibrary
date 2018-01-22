namespace DataGateway.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Rmf")]
    public partial class Rmf
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Rmf()
        {
            Books = new HashSet<Book>();
        }

        [Key]
        [StringLength(50)]
        public string IdRmf { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateIn { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateOut { get; set; }

        [Required]
        [StringLength(50)]
        public string DocId { get; set; }

        public int Quantity { get; set; }

        public decimal? TotalValue { get; set; }

        public int FirstInvNr { get; set; }

        public int LastInvNr { get; set; }

        [StringLength(50)]
        public string ContentCat { get; set; }

        [StringLength(50)]
        public string OutCause { get; set; }

        public bool? IsOut { get; set; }

        public byte[] DocImg { get; set; }

        public string Origin { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Book> Books { get; set; }
    }
}
