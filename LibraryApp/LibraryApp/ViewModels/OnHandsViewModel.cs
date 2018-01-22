using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using LibraryApp.SubServiceLayer;

namespace LibraryApp.ViewModels
{
    public class OnHandsViewModel
    {
        
       

        private ObservableCollection<OnHand> _onHandsBooks = new ObservableCollection<OnHand>();

        public ObservableCollection<OnHand> OnHandsBooks
        {
            get { return _onHandsBooks; }
            set { _onHandsBooks = value; }
        }


        public OnHandsViewModel()
        {
            Initialize();
        }


        private async void Initialize()
        {
            
        }
    }
}
