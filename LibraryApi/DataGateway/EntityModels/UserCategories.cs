using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataGateway.EntityModels
{
    public class UserCategory
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string CategoryTitle { get; set; }

        [ForeignKey("UserId")]
        [IgnoreDataMember]
        public virtual User User { get; set; } 
    }
}
