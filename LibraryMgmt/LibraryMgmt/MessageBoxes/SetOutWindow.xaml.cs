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

namespace LibraryMgmt.MessageBoxes
{
    /// <summary>
    /// Interaction logic for SetOutWindow.xaml
    /// </summary>
    public partial class SetOutWindow : Window
    {
        public string OutCause { get; set; }
        public SetOutWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (outCauseCombo.SelectedIndex == outCauseCombo.Items.Count-1)
            {
                if (!(String.IsNullOrEmpty(((TextBox)outCauseCombo.SelectedItem).Text)))
                {
                    OutCause = ((TextBox)outCauseCombo.SelectedItem).Text;
                    DialogResult = true;

                }
                else
                {
                    MessageBox.Show("Type something");
                }
            }
            else
            {
                if (outCauseCombo.SelectedItem!=null)
                {
                    OutCause = ((ComboBoxItem)outCauseCombo.SelectedItem).Content.ToString();
                    DialogResult = true; 
                }
                else
                {
                    MessageBox.Show("Select something!");
                }
            }
        }
    }
}
