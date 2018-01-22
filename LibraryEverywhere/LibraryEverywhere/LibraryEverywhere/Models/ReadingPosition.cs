using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEverywhere.Models
{
    public class ReadingPosition
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookCodeId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public DateTime SharingTime { get; set; }
        public virtual BookCode BookCode { get; set; }

        public virtual User User { get; set; }
    }
}
