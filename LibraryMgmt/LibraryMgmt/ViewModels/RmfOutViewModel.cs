using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryMgmt.Common;
using LibraryMgmt.ServiceReference;
using static LibraryMgmt.Common.ServiceClient;

namespace LibraryMgmt.ViewModels
{
    public class RmfOutViewModel : ObservableBase
    {
        private ObservableCollection<Rmf> _list = new ObservableCollection<Rmf>();
        public ObservableCollection<Rmf> List
        {
            get { return _list; }
            set { _list = value; NotifyPropertyChanged(nameof(List));}
        }

        private ObservableCollection<Rmf> _selectedItems = new ObservableCollection<Rmf>();
        public ObservableCollection<Rmf> SelectedItems
        {
            get { return _selectedItems; }
            set { _selectedItems = value; NotifyPropertyChanged(nameof(SelectedItems)); }
        }

        private Rmf _selectedItem;

        public Rmf SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; NotifyPropertyChanged(nameof(SelectedItem));}
        }

        public RmfOutViewModel()
        {
            ViewModelsGateway.RmfOutViewModel = this;
        }

        public async Task InitializeAsync()
        {
            List.Clear();
            foreach (var rmf in await GetOutRmfList())
            {
                List.Add(rmf);
            }
        }

        private async Task<IEnumerable<Rmf>> GetOutRmfList()
        {
            return await Client.GetOutRmfAsync();
        }

        public async Task DeleteSelected()
        {
            await Client.DeleteRmfAsync(SelectedItem.IdRmf);
            List.Remove(SelectedItem);
        }

        public async Task<bool> Delete(Rmf item)
        {
            var success = await Client.DeleteRmfAsync(item.IdRmf);
            if (success)
            {
                List.Remove(item);
            }

            return success;
        }

        public async Task Delete(IEnumerable<Rmf> items)
        {
            List<Rmf> list = items.ToList();
            foreach (var item in list)
            {
                await Delete(item);
            }
        }





    }
}
