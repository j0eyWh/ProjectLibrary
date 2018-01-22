using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryMgmt.ServiceReference;
using System.Collections.ObjectModel;
using System.ComponentModel;
using LibraryMgmt.Common;

namespace LibraryMgmt.ViewModels
{
    public class UsersViewModel:ObservableBase
    {
        private ObservableCollection<User> _usersList = new ObservableCollection<User>();
        public ObservableCollection<User> UserList
        {
            get { return _usersList; }
            set { _usersList = value; NotifyPropertyChanged(nameof(UserList)); }
        }

        private ObservableCollection<OnHand> _onHandList = new ObservableCollection<OnHand>();
        public ObservableCollection<OnHand> OnHandList
        {
            get { return _onHandList; }
            set { _onHandList = value; }
        }

        private ObservableCollection<BookCodeOwned> _booksPreviouslyOwned = new ObservableCollection<BookCodeOwned>();

        public ObservableCollection<BookCodeOwned> BooksPreviouslyOwned
        {
            get { return _booksPreviouslyOwned; }
            set { _booksPreviouslyOwned = value; }
        }

        private ObservableCollection<ReservedBook> _reservedBooks = new ObservableCollection<ReservedBook>();

        public ObservableCollection<ReservedBook> ReservedBooks
        {
            get { return _reservedBooks; }
            set { _reservedBooks = value; }
        }


        private User _selectedUser;

        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                NotifyPropertyChanged("SelectedUser");

                _onHandList.Clear();
                _booksPreviouslyOwned.Clear();
                _reservedBooks.Clear();

                if (_selectedUser!=null)
                {
                    GetUserOwnedBooks(SelectedUser.UserId);
                    GetUserReservedBooks(SelectedUser.UserId);
                    foreach (var item in Client.GetOnHands(_selectedUser))
                    {
                        _onHandList.Add(item);
                    } 
                }

            }
        }

        private OnHand _selectedOnHand;
                
        public OnHand SelectedOnHand
        {
            get { return _selectedOnHand; }
            set { _selectedOnHand = value; NotifyPropertyChanged(nameof(SelectedOnHand)); }
        }

        private BookCodeOwned _selectedBookCodeOwned;
        public BookCodeOwned SelectedBookCodeOwned
        {
            get { return _selectedBookCodeOwned; }
            set { _selectedBookCodeOwned = value; }
        }

        private ReservedBook _selectedReservedBook;
        public ReservedBook SelectedReservedBook
        {
            get { return _selectedReservedBook; }
            set { _selectedReservedBook = value; }
        }


        public async Task InitializeAsync()
        {
            foreach (var item in await Client.GetUsersAsync())
            {
                _usersList.Add(item);
            }
        }

        private async void GetUserOwnedBooks(int userId)
        {
            foreach (var item in await Client.GetUserOwnedBooksAsync(userId))
            {
                _booksPreviouslyOwned.Add(item);
            }
        }

        private async void GetUserReservedBooks(int userId)
        {
            foreach (var item in await Client.GetReservedBooksAsync(userId))
            {
                _reservedBooks.Add(item);
            }
        }

        public void Refresh()
        {
            if (SelectedUser == null) return;
            _booksPreviouslyOwned.Clear();
            _reservedBooks.Clear();
            GetUserOwnedBooks(SelectedUser.UserId);
            GetUserReservedBooks(SelectedUser.UserId);
        }

        public async void ResetUserList()
        {
            UserList.Clear();
            await InitializeAsync();
        }

    }
}
