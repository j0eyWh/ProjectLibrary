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
    /// Interaction logic for EditUserWindow.xaml
    /// </summary>
    public partial class EditUserWindow : Window
    {
        OpenFileDialog openDialog;

        public User EditedUser { get; set; }
        private User OldUser { get; set; }


        public EditUserWindow(User user)
        {
            InitializeComponent();

            openDialog = new OpenFileDialog();
            openDialog.Filter = "Images (.jpg, .png, .bmp)|*.jpg;*.bmp;*.png";

            OldUser = user;

            NameTb.Text = user.Name;
            SurnameTb.Text = user.Surname;
            DateTb.Text = user.BirthDate.ToShortDateString();
            IdnpTb.Text = user.Idnp;
            EmailTb.Text = user.Email;
            PhoneTb.Text = user.PhoneNumber;

            if (user.UserPic!=null)
            {
                MemoryStream stream = new MemoryStream(user.UserPic);
                image.Source = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }
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
                    email: EmailTb.Text,
                    phone: PhoneTb.Text
                    );

                if (openDialog.FileName == "")
                {
                    toBeAdded.UserPic = OldUser.UserPic;
                }

                toBeAdded.UserId = OldUser.UserId;

                Client.UpdateUser(toBeAdded);

                this.EditedUser = toBeAdded;

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
