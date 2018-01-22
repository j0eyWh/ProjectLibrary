using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEverywhere.Models
{
    public class UserCateogory
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string CategoryTitle { get; set; }
    }
}
