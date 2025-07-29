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
using ViolationManagement.Controllers;

namespace ViolationManagement.Views
{
    /// <summary>
    /// Interaction logic for AddAccount.xaml
    /// </summary>
    public partial class AddAccount : Window
    {
        private readonly RegisterController _controller = new();
        public AddAccount()
        {
            InitializeComponent();
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string cccd = CCCDTextBox.Text.Trim();
            string email = EmailTextBox.Text.Trim();
            string phone = PhoneTextBox.Text.Trim();
            string fullName = FullNameTextBox.Text.Trim();
            string role = RoleComboBox.Text;
            string gender = GenderComboBox.Text;
            string address = AddressTextBox.Text.Trim();

            bool success = _controller.AddAccount(cccd, email, phone, fullName, role , gender, address, out string message);

            if (success)
            {
                MessageBox.Show("Thêm tài khoản thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show(message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void GenderComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
        private void RoleComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
