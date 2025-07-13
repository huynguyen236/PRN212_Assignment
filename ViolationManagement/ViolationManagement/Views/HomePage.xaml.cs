using System;
using System.Windows;
using ViolationManagement.Helper;

namespace ViolationManagement.Views
{
    public partial class HomePage : Window
    {
        public HomePage()
        {
            InitializeComponent();

            LoadUserInfo();
        }

        private void LoadUserInfo()
        {
            if (UserSession.IsLoggedIn)
            {
                string? name = UserSession.GetClaim(System.Security.Claims.ClaimTypes.Name);
                string? role = UserSession.GetClaim(System.Security.Claims.ClaimTypes.Role);

                WelcomeText.Text = $"Xin chào, {name}!";

                switch (role)
                {
                    case "Admin":
                        AdminPanel.Visibility = Visibility.Visible;
                        break;
                    case "Police":
                        PolicePanel.Visibility = Visibility.Visible;
                        break;
                    case "Citizen":
                        CitizenPanel.Visibility = Visibility.Visible;
                        break;
                }
            }
            else
            {
                WelcomeText.Text = "Xin chào, khách!";
            }
        }

        private void OpenHome(object sender, RoutedEventArgs e)
        {
            // nothing
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
            var login = new Login();
            login.Show();
            this.Close();
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
