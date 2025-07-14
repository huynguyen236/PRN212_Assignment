using System.Windows;
using ViolationManagement.Controllers;
using ViolationManagement.Helper;

namespace ViolationManagement.Views
{
    public partial class RegisterPage : Window
    {
        private readonly RegisterController _controller = new();

        public RegisterPage()
        {
            InitializeComponent();

            if (UserSession.IsLoggedIn)
            {
                BtnRegister.Visibility = Visibility.Collapsed;
                BtnLogin.Visibility = Visibility.Collapsed;
                BtnLogout.Visibility = Visibility.Visible;
            }
            else
            {
                BtnRegister.Visibility = Visibility.Visible;
                BtnLogin.Visibility = Visibility.Visible;
                BtnLogout.Visibility = Visibility.Collapsed;
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string cccd = CCCDTextBox.Text.Trim();
            string email = EmailTextBox.Text.Trim();
            string phone = PhoneTextBox.Text.Trim();
            string fullName = FullNameTextBox.Text.Trim();
            string gender = GenderComboBox.Text;
            string address = AddressTextBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            bool success = _controller.Register(cccd, email, phone, fullName, gender, address, password, confirmPassword, out string message);

            if (success)
            {
                MessageBox.Show("Đăng ký thành công! Vui lòng đăng nhập.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
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
            MessageBox.Show("Chức năng đang được phát triển.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OpenRegister(object sender, RoutedEventArgs e)
        {
           // nothing
        }

        private void OpenLogin(object sender, RoutedEventArgs e)
        {
            var login = new Login();
            login.Show();
            this.Close();
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            UserSession.Logout();
            var home = new HomePage();
            home.Show();
            this.Close();
        }

        private void GenderComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
