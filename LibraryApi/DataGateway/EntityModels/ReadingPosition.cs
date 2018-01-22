using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataGateway.EntityModels
{
    public class ReadingPosition
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookCodeId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public DateTime SharingTime { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("BookCodeId")]
        public virtual BookCode BookCode { get; set; }

    }
}
