using System;
using System.Collections.Generic;

namespace LibraryEverywhere.Models
{
    public class Rmf
    {
        public Rmf()
        {
            Books = new HashSet<Book>();
        }

        public string IdRmf { get; set; }

        public DateTime DateIn { get; set; }

        public DateTime? DateOut { get; set; }

        public string DocId { get; set; }

        public int Quantity { get; set; }

        public decimal? TotalValue { get; set; }

        public int FirstInvNr { get; set; }

        public int LastInvNr { get; set; }

        public string ContentCat { get; set; }

        public string OutCause { get; set; }

        public bool? IsOut { get; set; }

        public byte[] DocImg { get; set; }

        public string Origin { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
