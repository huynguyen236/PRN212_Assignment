using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using ViolationManagement.Controller;
using ViolationManagement.Models;

namespace ViolationManagement.Views
{
    public partial class RegisterPage : Window
    {
        private readonly ViolationManagementContext _context = new ViolationManagementContext();
        private readonly RegisterControllers _controller = new();
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text.Trim();
            string phone = PhoneTextBox.Text.Trim();
            string fullName = FullNameTextBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            bool success = _controller.Register(email, phone, fullName, password, confirmPassword, out string message);

            if (success)
            {
                MessageBox.Show("Đăng ký thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                new Login().Show();
                this.Close();
            }
            else
            {
                MessageBox.Show(message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
 

        private void OpenHome(object sender, RoutedEventArgs e)
        {
            var home = new HomePage();
            home.Show();
            this.Close();
        }

        private void OpenLookup(object sender, RoutedEventArgs e)
        {
            //var lookup = new LookupPage();
            //lookup.Show();
            //this.Close();
        }

        private void OpenRegister(object sender, RoutedEventArgs e)
        {
            // đang ở trang Register
        }

        private void OpenLogin(object sender, RoutedEventArgs e)
        {
            var login = new Login();
            login.Show();
            this.Close();
        }
    }
}
