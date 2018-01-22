using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ProjectLibraryService
{
    public class ReservedBook
    {
        [Key]
        public int ReservationId { get; set; }

        public int UserId { get; set; }

        public int Code { get; set; }

        public DateTime TimeOut { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("Code")]
        public BookCode BookCode { get; set; }

    }
}
