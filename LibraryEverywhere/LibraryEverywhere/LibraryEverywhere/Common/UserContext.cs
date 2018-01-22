using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryEverywhere.Models;

namespace LibraryEverywhere.Common
{
    public class UserContext
    {
        public static User CurrentUser { get; private set; }

        public UserContext(User user)
        {
            CurrentUser = user;
        }
    }
}
