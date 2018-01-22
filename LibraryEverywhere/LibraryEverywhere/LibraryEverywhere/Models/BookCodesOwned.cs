using System;

namespace LibraryEverywhere.Models
{
    public class BookCodesOwned
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public int UserId { get; set; }
        public DateTime LendDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public virtual BookCode BookCode { get; set; }
        public virtual User User { get; set; }
    }
}
