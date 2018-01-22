using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataGateway.EntityModels
{
    public class BookTag
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public string Tag { get; set; }

        [ForeignKey("BookId")]
        [IgnoreDataMember]
        public Book Book { get; set; }
    }
}
