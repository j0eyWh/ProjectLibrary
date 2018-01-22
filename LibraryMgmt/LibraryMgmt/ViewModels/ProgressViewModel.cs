using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LibraryMgmt.Common;

namespace LibraryMgmt.ViewModels
{
    public class ProgressViewModel:ObservableBase
    {
        private string _progressMessage;

        public string ProgressMessage
        {
            get { return _progressMessage; }
            set { _progressMessage = value; NotifyPropertyChanged(nameof(ProgressMessage)); }
        }

        private Visibility _visibility = Visibility.Hidden;

        public Visibility Visibility
        {
            get { return _visibility; }
            set { _visibility = value; NotifyPropertyChanged(nameof(Visibility));}
        }

        public ProgressViewModel()
        {
            ViewModelsGateway.ProgressViewModel = this;
        }

    }
}
