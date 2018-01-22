namespace ProjectLibraryService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using GHM;
    using System.Runtime.Serialization;
    using System.Linq;
    using System.ServiceModel;

    [Table("Rmf")]
    public partial class Rmf
    {
        
        [Key]
        [StringLength(50)]
        public string IdRmf { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateIn { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateOut { get; set; }

        [Required]
        [StringLength(50)]
        [Index(IsUnique = true)]
        public string DocId { get; set; }

        //TODO: Drop in the next migration
        //Cause: data redundancy
        //[DataMember]
        //[StringLength(50)]
        //public string DocOutId { get; set; }

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

       // public List<Book> BookList { get; }

       
        //    public Rmf()
        //    {

        //    }


        //    public Rmf(string docId)
        //    {
        ////        this.IdRmf = Hm.GenerateNewId(new LibraryService().GetLastIndex());
        ////        this.DateIn = DateTime.Now;
        ////        this.DateOut = null;

        ////        //if (
        ////        //    !String.IsNullOrWhiteSpace(docId) &&
        ////        //    new LibraryService().
        ////        //    )
        ////        //{
        ////        //    this.DocId = docId;
        ////        //}
        ////        //else
        ////        //    throw new ArgumentException(message: $"The document id is not valid or already exists");
        //    }
    }
}
