using Microsoft.Win32;
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
using System.IO;
using static LibraryMgmt.Common.ServiceClient;

namespace LibraryMgmt.MessageBoxes
{
    /// <summary>
    /// Interaction logic for AddNewUserWindow.xaml
    /// </summary>
    public partial class AddNewUserWindow : Window
    {
        OpenFileDialog openDialog;

        public User NewUser { get; set; }

        public AddNewUserWindow()
        {
            InitializeComponent();

            openDialog = new OpenFileDialog();
            openDialog.Filter = "Images (.jpg, .png, .bmp)|*.jpg;*.bmp;*.png";
        }

        private void OpenBtn_Click(object sender, RoutedEventArgs e)
        {
            if (openDialog.ShowDialog() == true)
            {
                FileStream f = new FileStream(openDialog.FileName, FileMode.Open, FileAccess.Read);
                image.Source = BitmapFrame.Create(f, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                User toBeAdded = new User(
                    name: NameTb.Text,
                    surname: SurnameTb.Text,
                    imgPath: openDialog.FileName,
                    birthDate: Convert.ToDateTime(DateTb.Text),
                    idnp: IdnpTb.Text,
                    phone: PhoneTb.Text,
                    email: EmailTb.Text
                    );

                Client.AddUser(toBeAdded);
                toBeAdded.UserId = Client.GetUsers().OrderByDescending(x => x.UserId).First().UserId;
                this.NewUser = toBeAdded;
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
