using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryMgmt.ViewModels;

namespace LibraryMgmt.Common
{
    static class ViewModelsGateway
    {
        public static RmfInViewModel RmfInViewModel { get; set; }
        public static RmfOutViewModel RmfOutViewModel { get; set; }
        public static ProgressViewModel ProgressViewModel { get; set; }
    }
}
