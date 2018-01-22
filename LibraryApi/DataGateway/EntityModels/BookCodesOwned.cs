using System.Runtime.Serialization;

namespace DataGateway.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BookCodesOwned")]
    public partial class BookCodesOwned
    {
        public int Id { get; set; }

        public int Code { get; set; }

        public int UserId { get; set; }

        public DateTime LendDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public virtual BookCode BookCode { get; set; }

        [IgnoreDataMember]
        public virtual User User { get; set; }
    }
}
