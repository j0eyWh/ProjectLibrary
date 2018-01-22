using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryMgmt.Common;
using LibraryMgmt.ServiceReference;
using LibraryMgmt.Common;

namespace LibraryMgmt.ViewModels
{
    public class RmfInViewModel : ObservableBase
    {
        private ObservableCollection<Rmf> _rmfList = new ObservableCollection<Rmf>();
        public ObservableCollection<Rmf> RmfInList
        {
            get { return _rmfList; }
            set { _rmfList = value; NotifyPropertyChanged(nameof(RmfInList)); }
        }

        private ObservableCollection<Rmf> _selectedItems = new ObservableCollection<Rmf>();

        public ObservableCollection<Rmf> SelectedItems
        {
            get { return _selectedItems; }
            set { _selectedItems = value; NotifyPropertyChanged(nameof(SelectedItems)); }
        }



        private Rmf _selectedItem = null;

        public Rmf SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; NotifyPropertyChanged(nameof(SelectedItem)); }
        }


        public RmfInViewModel()
        {
            ViewModelsGateway.RmfInViewModel = this;
        }

        public async Task InitializeAsync()
        {
            RmfInList.Clear();
            foreach (var rmf in await GetRmfInListAsync())
            {
                RmfInList.Add(rmf);
            }
        }

        private async Task<IEnumerable<Rmf>> GetRmfInListAsync()
        {
            return await Client.GetInRmfAsync();
        }

        public async Task<bool> DeleteSelected()
        {
            if (SelectedItems.Count==1)
            {
                var success = await Client.DeleteRmfAsync(SelectedItem.IdRmf);
                if (success)
                    RmfInList.Remove(SelectedItem);
                return success;
            }

            bool r=false;

            List<Rmf> temp = SelectedItems.ToList();

            foreach (var selectedItem in temp)
            {
                r = await Client.DeleteRmfAsync(selectedItem.IdRmf);
                RmfInList.Remove(selectedItem);
            }

            //SelectedItems.Clear();

            //await InitializeAsync();

            return r;
        }

        public async Task<Rmf> SetSelectedOut(string cause)
        {
            if (SelectedItem == null) return null;

            SelectedItem.IsOut = true;
            SelectedItem.DateOut = DateTime.Now;
            SelectedItem.OutCause = cause;

            var success = await Client.UpdateRmfAsync(SelectedItem);

            if (!success) return null;
            Rmf r= new Rmf();
            r = SelectedItem;
            RmfInList.Remove(SelectedItem);
            return r;
        }
    }
}
