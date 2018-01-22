using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace ProjectLibraryService
{
    
    public partial class User
    {
        
        public int UserId { get; set; }

        public string Name { get; set; }

        
        public string Surname { get; set; }

        public string Idnp { get; set; }

        public byte[] UserPic { get; set; }

        public DateTime BirthDate { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}
