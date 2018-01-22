using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryApp.SubServiceLayer;

namespace LibraryApp
{
    static class UserContext
    {
        public static User CurrentUser { get; set; } = null;

    }
}
