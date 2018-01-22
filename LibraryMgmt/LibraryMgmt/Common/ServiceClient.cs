using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryMgmt.ServiceReference;

namespace LibraryMgmt.Common
{
    static class ServiceClient
    {
        public static LibraryServiceClient Client => new LibraryServiceClient();
    }
}
