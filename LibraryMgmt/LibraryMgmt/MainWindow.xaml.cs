using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Ribbon;
using System.Windows.Forms;
using LibraryMgmt.Common;
using LibraryMgmt.ServiceReference;
using static LibraryMgmt.Common.ServiceClient;
using Binding = System.Windows.Data.Binding;
using Brush = System.Windows.Media.Brush;
using ListView = System.Windows.Controls.ListView;
using MessageBox = System.Windows.MessageBox;
using TabControl = System.Windows.Controls.TabControl;

namespace LibraryMgmt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RibbonWin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private async void newRmfButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxes.NewItemWindow messageBox = new MessageBoxes.NewItemWindow
            {
                Owner = this,
                Margin = this.Margin
            };
            messageBox.ShowDialog();

            if (messageBox.DialogResult != true) return;

            var rmf = messageBox.generatedRmf;

            await Client.InsertRmfAsync(rmf); 
            RmfInViewModel.RmfInList.Add(rmf);
        }

        /// <summary>
        /// Delete objects from ViewModel.ListView.RmfIn
        /// </summary>
        /// TODO: handle removing multiple objects
        private async void deleteSelectedBtn_Click(object sender, RoutedEventArgs e)
        {
            switch (mainTabControl.SelectedIndex)
            {
                case 0:
                    {

                        if (listView.SelectedItems.Count==1)
                        {
                            if (RmfInViewModel.SelectedItem == null)
                            {
                                 MessageBox.Show("Please, select an element first!", "No element selected!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            }
                            else
                            {
                                if (MessageBox.Show($"Are you sure you want to delete {RmfInViewModel.SelectedItem.IdRmf} item? \n It wouldn't be possibile to restore it!",
                                    "Confirm deleting",
                                    MessageBoxButton.YesNo,
                                    MessageBoxImage.Warning) == MessageBoxResult.Yes
                                    )
                                {
                                    await RmfInViewModel.DeleteSelected();
                                }
                            } 
                        }

                        else
                        {
                            if (MessageBox.Show($"Are you sure you want to delete {RmfInViewModel.SelectedItems.Count} items? \n It wouldn't be possibile to restore them!",
                                    "Confirm deleting",
                                    MessageBoxButton.YesNo,
                                    MessageBoxImage.Warning) == MessageBoxResult.Yes
                                    )
                            {
                                await RmfInViewModel.DeleteSelected();
                            }
                        }
                        break;
                    }
                case 1:
                    {
                        if (RmfOutViewModel.SelectedItems.Count<=1)
                        {
                            if (RmfOutViewModel.SelectedItem == null)
                            {
                                MessageBox.Show("Please, select an element first!", "No element selected!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            }
                            else
                            {
                                if (MessageBox.Show($"Are you sure you want to delete {RmfOutViewModel.SelectedItem.IdRmf} item? \n It wouldn't be possibile to restore it!",
                                        "Confirm deleting",
                                        MessageBoxButton.YesNo,
                                        MessageBoxImage.Warning) == MessageBoxResult.Yes
                                        )
                                {
                                    await RmfOutViewModel.Delete(RmfOutViewModel.SelectedItem);
                                }
                            } 
                        }
                        else
                        {
                            if (MessageBox.Show($"Are you sure you want to delete {RmfOutViewModel.SelectedItems.Count} items? \n It wouldn't be possibile to restore them!",
                                    "Confirm deleting",
                                    MessageBoxButton.YesNo,
                                    MessageBoxImage.Warning) == MessageBoxResult.Yes
                                    )
                            {
                                await RmfOutViewModel.Delete(RmfOutViewModel.SelectedItems);
                            }
                        }
                        break;

                    }
            }

            
        }

        private void importFromScratchBtn_Click(object sender, RoutedEventArgs e)
        {
            var messageBox = new MessageBoxes.ImportFromScratchWindow {Owner = this};
            messageBox.ShowDialog();

            if (messageBox.DialogResult != true) return;

            if (messageBox.generatedRmf.IsOut == true)
                RmfOutViewModel.List.Add(messageBox.generatedRmf);
            else
                RmfInViewModel.RmfInList.Add(messageBox.generatedRmf);
            
        }

        private async void getOutBtn_Click(object sender, RoutedEventArgs e)
        {

            if (listView.SelectedItems.Count<=1)
            {
                if (RmfInViewModel.SelectedItem != null)
                {
                    if (MessageBox.Show(
                        "Are you sure you want to set this record out? It wouldn't be posible to set it back!",
                        "Warning!!!",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning) != MessageBoxResult.Yes) return;

                    var messageBox = new MessageBoxes.SetOutWindow {Owner = this};
                    messageBox.ShowDialog();

                    if (messageBox.DialogResult != true) return;

                    var rmf = await RmfInViewModel.SetSelectedOut(messageBox.OutCause);

                    if (rmf!=null)
                        RmfOutViewModel.List.Add(rmf); 

                }
                else
                {
                    MessageBox.Show("Select an element first");
                } 
            }
            else
            {
                MessageBox.Show("You can't unregister multiple records. Select only one", "Warning!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }

        private void generalReportBtn_Click(object sender, RoutedEventArgs e)
        {
            (new ReportWindows.GeneralInReportWindow()).Show();
        }

        private void anualReportButton_Click(object sender, RoutedEventArgs e)
        {
            (new ReportWindows.AnualReportWindow()).Show();
        }

        private void aboutRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            switch (mainTabControl.SelectedIndex)
            {
                case 0:
                    {
                        if (RmfInViewModel.SelectedItem==null)
                        {
                            MessageBox.Show("Select a record first!");
                            return;                           
                        }

                        (new ReportWindows.DetailsReportWindow(RmfInViewModel.SelectedItem)).Show();
                        break;
                    }
                case 1:
                    {
                        if (RmfOutViewModel.SelectedItem == null)
                        {
                            MessageBox.Show("Select a record first!");
                            return;
                        }

                        (new ReportWindows.DetailsReportWindow(RmfOutViewModel.SelectedItem)).Show(); 
                        break;
                    }
            }

            
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        { 
            this.Close();
        }

        private void helpButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Help.ShowHelp(null, "help.chm");
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.RmfInViewModel.SelectedItems.Clear();
            if (listView.SelectedItems == null) return;

            foreach (var item in listView.SelectedItems)
            {
                RmfInViewModel.SelectedItems.Add((Rmf)item);

            }
        }

        private void listViewOut_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewOut.SelectedItems == null) return;

            RmfOutViewModel.SelectedItems.Clear();

            foreach (var item in listViewOut.SelectedItems)
            {
                RmfOutViewModel.SelectedItems.Add((Rmf)item);
            }
        }

        private void mainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabControl t = (TabControl)sender;
            TabItem i = (TabItem)(t.SelectedItem);

            switch (i.Name)
            {
                case "RmfInTab":
                    {
                        RmfTabGroup.Visibility = Visibility.Visible;
                        BooksTabGroup.Visibility = Visibility.Collapsed;
                        UsersTabGroup.Visibility = Visibility.Collapsed;
                        break;
                    }

                case "RmfOutTab":
                    {
                        RmfTabGroup.Visibility = Visibility.Visible;
                        BooksTabGroup.Visibility = Visibility.Collapsed;
                        UsersTabGroup.Visibility = Visibility.Collapsed;
                        break;
                    }

                case "BooksTab":
                    {
                        RmfTabGroup.Visibility = Visibility.Collapsed;
                        BooksTabGroup.Visibility = Visibility.Visible;
                        UsersTabGroup.Visibility = Visibility.Collapsed;
                        //BrushConverter b = new BrushConverter();
                        //Brush s = (Brush)b.ConvertFrom("#FF68217A");
                        //StatusBr.Background = s;
                        break;
                    }
                case "UsersTab":
                    {
                        RmfTabGroup.Visibility = Visibility.Collapsed;
                        BooksTabGroup.Visibility = Visibility.Collapsed;
                        UsersTabGroup.Visibility = Visibility.Visible;
                        break;
                    }
                default:
                    {
                        //BrushConverter b = new BrushConverter();
                        //StatusBr.Background = (Brush)b.ConvertFrom("#FF007ACC");
                        break;
                    }
                    
            }

        }

        private void NewBookBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxes.AddNewBookWindow messageBox = new MessageBoxes.AddNewBookWindow();
            messageBox.Margin = this.Margin;
            messageBox.ShowDialog();

            if (messageBox.DialogResult == true)
            {
                BooksViewModel.BookList.Add(messageBox.NewBook);
            }
            
        }

        private void EditBookBtn_Click(object sender, RoutedEventArgs e)
        {
            if (BooksViewModel.SelectedBook!=null)
            {
                MessageBoxes.EditBookWindow messageBox = new MessageBoxes.EditBookWindow(BooksViewModel.SelectedBook);
                messageBox.ShowDialog();
                int index = BooksViewModel.BookList.IndexOf(BooksViewModel.SelectedBook);

                if (messageBox.DialogResult == true)
                {
                    BooksViewModel.BookList[index] = messageBox.UpdatedBook;
                }
                
            }
            else
            {
                MessageBox.Show("Select a book first");
            }

        }

        private void DeleteBookBtn_Click(object sender, RoutedEventArgs e)
        {
            if (listViewBooks.SelectedItem!=null)
            {
                if (!Client.GetBookCodesOnHand(BooksViewModel.SelectedBook).Any())
                {
                    if (MessageBox.Show($"Are you sure you want to delete {BooksViewModel.SelectedBook.Title}",
                                "Warning!!",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                       
                        Client.DeleteBookById(BooksViewModel.SelectedBook.BookId);
                        BooksViewModel.BookList.Remove(BooksViewModel.SelectedBook);

                        if (BooksViewModel.SearchResults!=null && BooksViewModel.SearchResults.Contains(BooksViewModel.SelectedBook))
                        {
                            BooksViewModel.SearchResults.Remove(BooksViewModel.SelectedBook);

                        }

                    } 
                }
                else
                {
                    MessageBox.Show("This book still has volumes on hands");
                }
            }
            else
            {
                MessageBox.Show("Select a book first");
            }
        }

        private void Ribbon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedName;
            if (((RibbonTab)RibbonW.SelectedItem) !=null)
            {
                selectedName = ((RibbonTab)RibbonW.SelectedItem).Name;
            }
            else
            {
                selectedName = null;
            }

            switch (selectedName)
            {
                case "BooksMgmtTab":
                    {
                        BrushConverter b = new BrushConverter();
                        StatusBr.Background = (Brush)b.ConvertFrom("#FFCA5100");
                        break;
                    }
                case "ToolsTab":
                case "ReportsTab":
                    {
                        BrushConverter b = new BrushConverter();
                        StatusBr.Background = (Brush)b.ConvertFrom("#FF68217A");
                        break;
                    }
                case "UserToolsTab":
                    {
                        BrushConverter b = new BrushConverter();
                        StatusBr.Background = (Brush)b.ConvertFrom("#FF4B8511");
                        break;
                    }
                default:
                    {
                        BrushConverter b = new BrushConverter();
                        StatusBr.Background = (Brush)b.ConvertFrom("#FF007ACC");
                        break;
                    }
            }

            //MessageBox.Show(selectedName);


        }

        private void BookSearchBtn_Click(object sender, RoutedEventArgs e)
        {
            var searchString = BookSearchTb.Text.ToLower();

            List<Book> results = BooksViewModel.BookList.Where(x =>
                x.Author.ToLower().Contains(searchString)
                || x.Title.ToLower().Contains(searchString)
                || x.Category.ToLower().Contains(searchString)
                || x.Publisher.Contains(searchString)

                ).ToList();

            BooksViewModel.SearchResults = new System.Collections.ObjectModel.ObservableCollection<Book>(results);

            listViewBooks.ItemsSource = BooksViewModel.SearchResults;
        }

        private void UndoBookSearchBtn_Click(object sender, RoutedEventArgs e)
        {
            listViewBooks.ItemsSource = BooksViewModel.BookList;
            BookSearchTb.Text = "";
        }

        private void AddUserBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxes.AddNewUserWindow messageBox = new MessageBoxes.AddNewUserWindow();
            messageBox.ShowDialog();

            if (messageBox.DialogResult == true)
                UsersViewModel.UserList.Add(messageBox.NewUser);
            

        }

        private void EditUserBtn_Click(object sender, RoutedEventArgs e)
        {
            if (UsersViewModel.SelectedUser!=null)
            {
                MessageBoxes.EditUserWindow messageBox = new MessageBoxes.EditUserWindow(UsersViewModel.SelectedUser);
                messageBox.ShowDialog();

                if (messageBox.DialogResult == true)
                {
                    int index = UsersViewModel.UserList.IndexOf(UsersViewModel.SelectedUser);
                    UsersViewModel.UserList[index] = messageBox.EditedUser;
                }

            }
            else
            {
                MessageBox.Show("Select a user first");
            }
        }

        private void DeleteUserBnt_Click(object sender, RoutedEventArgs e)
        {
            if (UsersViewModel.SelectedUser!=null)
            {
                if (!Client.GetOnHands(UsersViewModel.SelectedUser).Any())
                {
                    if (MessageBox.Show($"Are you sure you want to delete this user {UsersViewModel.SelectedUser.UserId}?",
                                "Warnning",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Warning
                                ) == MessageBoxResult.Yes
                               )
                    {
                        Client.DeleteUser(UsersViewModel.SelectedUser);
                        UsersViewModel.UserList.Remove(UsersViewModel.SelectedUser);
                    } 
                }
                else
                {
                    MessageBox.Show("This user still has books on hands!");
                }
            }

            else
            {
                MessageBox.Show("Please select an user first");
            }
        }

        private void LendBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxes.LendBookWindow messageBox = new MessageBoxes.LendBookWindow(BooksViewModel.SelectedBook);

            messageBox.ShowDialog();

            if (messageBox.DialogResult == true)
            {
                BooksViewModel.SelectedBookAvailableAmount = BooksViewModel.SelectedBookAvailableAmount - 1;
                if (UsersViewModel.SelectedUser!=null && UsersViewModel.SelectedUser.UserId == Convert.ToInt32(messageBox.usersComboBox.SelectedItem))
                {
                    UsersViewModel.OnHandList.Add(messageBox.AddedOnHand); 
                }
            }
        }

        private void ReturnBookBtn_Click(object sender, RoutedEventArgs e)
        {
            if (UsersViewModel.SelectedOnHand!=null)
            {
                try
                {
                    Client.DeleteOnHand(UsersViewModel.SelectedOnHand);
                    if (BooksViewModel.SelectedBook!=null && BooksViewModel.SelectedBook.BookId == UsersViewModel.SelectedOnHand.Book.BookId)
                    {
                        BooksViewModel.SelectedBookAvailableAmount = BooksViewModel.SelectedBookAvailableAmount + 1;
                    }
                    UsersViewModel.OnHandList.Remove(UsersViewModel.SelectedOnHand);
                    UsersViewModel.Refresh();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Select a book first!");
            }
        }

        private void GeneralBooksReportBtn_Click(object sender, RoutedEventArgs e)
        {
            ReportWindows.GeneralBooksReport reportWindow = new ReportWindows.GeneralBooksReport();

            reportWindow.Show();
        }

        private void OnHandsBooksButton_Click(object sender, RoutedEventArgs e)
        {
            Binding binding = new Binding()
            {
                Source = UsersViewModel,
                Path = new PropertyPath("SelectedOnHand"),
                Mode = BindingMode.Default,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                
            };

            UserBooksListView.ItemsSource = UsersViewModel.OnHandList;
            BindingOperations.SetBinding(UserBooksListView, ListView.SelectedItemProperty, binding);
            SetReturnedBtn.IsEnabled = true;
            CheckOutReserved.IsEnabled = false;
        }

        private void OwnedBooksButton_Click(object sender, RoutedEventArgs e)
        {
            Binding binding = new Binding()
            {
                Source = UsersViewModel,
                Path = new PropertyPath("SelectedBookCodeOwned"),
                Mode= BindingMode.Default,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
 
            };

            UserBooksListView.ItemsSource = UsersViewModel.BooksPreviouslyOwned;
            BindingOperations.SetBinding(UserBooksListView, ListView.SelectedItemProperty, binding);
            SetReturnedBtn.IsEnabled = false;
            CheckOutReserved.IsEnabled = false;
        }

        private void ReservedBooksButton_Click(object sender, RoutedEventArgs e)
        {
            Binding binding = new Binding()
            {
                Source = UsersViewModel,
                Path = new PropertyPath("SelectedReservedBook"),
                Mode = BindingMode.Default,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            UserBooksListView.ItemsSource = UsersViewModel.ReservedBooks;
            BindingOperations.SetBinding(UserBooksListView, ListView.SelectedItemProperty, binding);
            SetReturnedBtn.IsEnabled = false;
            CheckOutReserved.IsEnabled = true;
        }

        private void CheckOutReserved_Click(object sender, RoutedEventArgs e)
        {
            if (UsersViewModel.SelectedReservedBook!=null)
            {
                MessageBoxes.BookCheckOutWindow messageBox = new MessageBoxes.BookCheckOutWindow(UsersViewModel.SelectedReservedBook);
                messageBox.ShowDialog();
                if (messageBox.DialogResult == true)
                {
                    UsersViewModel.Refresh();
                }

            }
            else
            {
                MessageBox.Show("Select a reservation first!", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void SearchUsersBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var searchString = SearchUserTextBox.Text.ToLower();

            var users =
                UsersViewModel.UserList.Where(
                    x =>
                        x.Name.ToLower() == searchString || x.Surname.ToLower().Contains(searchString) ||
                        x.Idnp.ToLower().Contains(searchString));

            UsersViewModel.UserList = new ObservableCollection<User>(users);
        }

        private void ResetUserListBtn_OnClick_Click(object sender, RoutedEventArgs e)
        {
            UsersViewModel.ResetUserList();
        }

        private async void ReservationSatellite_Click(object sender, RoutedEventArgs e)
        {
            if (ServicesViewModel.ReservationSatelliteState)
            {
                await ServicesViewModel.StopReservationSatellite();
                return;
            }
            await ServicesViewModel.StartReservationSatellite();


        }

        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        { 
            ViewModelsGateway.ProgressViewModel.ProgressMessage = "Loading stuff...";
            ViewModelsGateway.ProgressViewModel.Visibility = Visibility.Visible;
            await RmfInViewModel.InitializeAsync();
            await Task.Delay(TimeSpan.FromSeconds(1));
            await RmfOutViewModel.InitializeAsync();
            await Task.Delay(TimeSpan.FromSeconds(1));
            await BooksViewModel.InitializeAsyc();
            await Task.Delay(TimeSpan.FromSeconds(1));
            await ServicesViewModel.InitializeAsync();
            await Task.Delay(TimeSpan.FromSeconds(1));
            await UsersViewModel.InitializeAsync();

            ViewModelsGateway.ProgressViewModel.Visibility = Visibility.Hidden;

   
        }
    }
}
