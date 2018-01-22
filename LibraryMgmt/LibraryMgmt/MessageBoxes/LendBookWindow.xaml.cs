using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using LibraryMgmt.ServiceReference;
using static LibraryMgmt.Common.ServiceClient;

namespace LibraryMgmt.MessageBoxes
{
    /// <summary>
    /// Interaction logic for LendBookWindow.xaml
    /// </summary>
    public partial class LendBookWindow : Window
    {

        public OnHand AddedOnHand { get; set; }

        private Book Book { get; set; }


        public LendBookWindow(Book book)
        {
            InitializeComponent();

            Book = book;

            foreach (var user in Client.GetUsers())
            {
                usersComboBox.Items.Add(user.UserId);
            }

           

            foreach (var code in Client.GetBookCodesNotOnHand(book))
            {
                codesComboBox.Items.Add(code.Code);
            }
            

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OnHand _toBeIntroduced = new OnHand()
                {
                    UserId = Convert.ToInt32(usersComboBox.SelectedItem),
                    Code = Convert.ToInt32(codesComboBox.SelectedItem),
                    ReturnDate = (DateTime)retDatePk.SelectedDate,
                    LendDate = DateTime.Now
                    
                };

                Client.AddReg(_toBeIntroduced);
                _toBeIntroduced.OnHandId = Client.GetAllOnHands().First().OnHandId;

                AddedOnHand = Client.GetBookCodesOnHand(Book).OrderByDescending(x => x.OnHandId).First();

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void usersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Client.GetUserById(Convert.ToInt32(usersComboBox.SelectedItem)) != null)
                {
                    UserNameLbl.Content = Client.GetUserById(Convert.ToInt32(usersComboBox.SelectedItem)).Name + " " + Client.GetUserById(Convert.ToInt32(usersComboBox.SelectedItem)).Surname;
                }
            }
            catch (Exception)
            {
                UserNameLbl.Content = "Utilizator neidentificat";
            }

            
        }
    }
}
