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
    [Table("BookCodesOwned")]
    public class BookCodeOwned
    {
        public int Id { get; set; }

        public int Code { get; set; }

        public int UserId { get; set; }

        public DateTime LendDate { get; set; }
        public DateTime ReturnDate { get; set; }

        public BookCode BookCode { get; set; }

        public User User { get; set; }


    }
}
