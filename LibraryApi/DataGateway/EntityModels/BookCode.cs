using System.Runtime.Serialization;

namespace DataGateway.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BookCode
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BookCode()
        {
            BookCodesOwneds = new HashSet<BookCodesOwned>();
            OnHands = new HashSet<OnHand>();
            ReservedBooks = new HashSet<ReservedBook>();
        }

        [Key]
        public int Code { get; set; }

        public int BookId { get; set; }

        public virtual Book Book { get; set; }

        [IgnoreDataMember]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookCodesOwned> BookCodesOwneds { get; set; }

        [IgnoreDataMember]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OnHand> OnHands { get; set; }

        [IgnoreDataMember]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReservedBook> ReservedBooks { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<ReadingPosition> ReadingPositions  { get; set; }
    }
}
