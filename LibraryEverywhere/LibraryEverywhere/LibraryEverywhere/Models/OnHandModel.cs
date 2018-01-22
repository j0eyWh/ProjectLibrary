using System;

namespace LibraryEverywhere.Models
{
    public class OnHand
    {
        public int OnHandId { get; set; }
        public int UserId { get; set; }
        public int Code { get; set; }
        public DateTime LendDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public virtual BookCode BookCode { get; set; }
        public virtual User User { get; set; }

    }
}
