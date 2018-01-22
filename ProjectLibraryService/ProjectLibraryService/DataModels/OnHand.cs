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
    
    public partial class OnHand
    {
        public int OnHandId { get; set; }

        public int UserId { get; set; }

        public int Code { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("Code")]
        public BookCode BookCode
        {
            get;

            set;
        }

        [NotMapped]
        public Book Book
        {
            get
            {
                int id = (new RmfModel()).BookCodes.Where(x => x.Code == this.Code).Single().BookId;
                return (new RmfModel()).Books.Where(x => x.BookId == id).Single();
            }
            set { }
        }

        public DateTime LendDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
