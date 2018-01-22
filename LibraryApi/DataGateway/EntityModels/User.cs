using System.Runtime.Serialization;

namespace DataGateway.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            BookCodesOwneds = new HashSet<BookCodesOwned>();
            OnHands = new HashSet<OnHand>();
            ReservedBooks = new HashSet<ReservedBook>();
        }

        public int UserId { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Idnp { get; set; }

        public byte[] UserPic { get; set; }

        public DateTime BirthDate { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [IgnoreDataMember]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookCodesOwned> BookCodesOwneds { get; set; }

        [IgnoreDataMember]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OnHand> OnHands { get; set; }

        [IgnoreDataMember]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReservedBook> ReservedBooks { get; set; }

        public virtual ICollection<UserCategory> Categories { get; set; }
        [IgnoreDataMember]
        public virtual ICollection<ReadingPosition> ReadingPositions { get; set; } 
    }
}
