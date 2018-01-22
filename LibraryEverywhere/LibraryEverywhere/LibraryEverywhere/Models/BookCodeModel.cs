using System.Collections.Generic;

namespace LibraryEverywhere.Models
{
    public class BookCode
    {
        public BookCode()
        {
            BookCodesOwneds = new HashSet<BookCodesOwned>();
            OnHands = new HashSet<OnHand>();
            ReservedBooks = new HashSet<ReservedBook>();
        }


        public int Code { get; set; }
        public int BookId { get; set; }
        public virtual Book Book { get; set; }

        public virtual ICollection<BookCodesOwned> BookCodesOwneds { get; set; }
        public virtual ICollection<OnHand> OnHands { get; set; }
        public virtual ICollection<ReservedBook> ReservedBooks { get; set; }
    }
}
