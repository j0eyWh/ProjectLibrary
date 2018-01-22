using System.Runtime.Serialization;

namespace DataGateway.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ReservedBook
    {
        [Key]
        public int ReservationId { get; set; }

        public int UserId { get; set; }

        public int Code { get; set; }

        public DateTime TimeOut { get; set; }

        public virtual BookCode BookCode { get; set; }

        [IgnoreDataMember]
        public virtual User User { get; set; }
    }
}
