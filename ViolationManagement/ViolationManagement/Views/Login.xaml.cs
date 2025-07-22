using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ViolationManagement.Controllers;
using ViolationManagement.Helper;

namespace ViolationManagement.Views
{
    public partial class Login : Window
    {
        private readonly LoginController _controller = new();

        public Login()
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

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string cccd = txtCCCD.Text.Trim();
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(cccd) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ CCCD và mật khẩu.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var user = _controller.Login(cccd, password);
            if (user != null)
            {
                // Set thông tin vào session
                UserSession.SetUser(user.UserId, user.FullName, user.Email, user.Role);

                MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                var home = new HomePage();
                home.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Email hoặc mật khẩu không đúng.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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
            var register = new RegisterPage();
            register.Show();
            this.Close();
        }

        private void OpenLogin(object sender, RoutedEventArgs e)
        {
            // nothing
        }

        private void ForgotPassword_Click(object sender, MouseButtonEventArgs e)
        {
            var forgotpassword = new ForgotPassword();
            forgotpassword.ShowDialog();
            //this.Close();
        }

        private void OpenRegister_Click(object sender, RoutedEventArgs e)
        {
            var register = new RegisterPage();
            register.Show();
            this.Close();
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Có thể validate real-time nếu muốn
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            UserSession.Logout();

            MessageBox.Show("Bạn đã đăng xuất.", "Đăng xuất", MessageBoxButton.OK, MessageBoxImage.Information);

            var login = new Login();
            login.Show();
            this.Close();
        }
    }
}
