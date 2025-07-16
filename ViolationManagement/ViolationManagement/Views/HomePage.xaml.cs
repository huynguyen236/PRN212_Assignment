using System;
using System.Windows;
using System.Windows.Controls;
using ViolationManagement.Helper;

namespace ViolationManagement.Views
{
    public partial class HomePage : Window
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bool isLoggedIn = UserSession.CurrentUser != null;

            btnLogin.Visibility = isLoggedIn ? Visibility.Collapsed : Visibility.Visible;
            btnRegister.Visibility = isLoggedIn ? Visibility.Collapsed : Visibility.Visible;
            btnProfile.Visibility = isLoggedIn ? Visibility.Visible : Visibility.Collapsed;
            btnLogout.Visibility = isLoggedIn ? Visibility.Visible : Visibility.Collapsed;

            LoadUserInfo();
        }

        private void LoadUserInfo()
        {
            if (UserSession.IsLoggedIn)
            {
                string? name = UserSession.GetClaim(System.Security.Claims.ClaimTypes.Name);
                string? role = UserSession.GetClaim(System.Security.Claims.ClaimTypes.Role);

                WelcomeText.Text = $"Xin chào, {name}!";

                btnRegister.Visibility = Visibility.Collapsed;
                btnLogin.Visibility = Visibility.Collapsed;
                btnLogout.Visibility = Visibility.Visible;

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

                // Cập nhật chức năng cho combobox
                FeatureComboBox.Items.Clear();
                FeatureComboBox.Visibility = Visibility.Visible;

                if (role == "Citizen")
                {
                    FeatureComboBox.Items.Add(new ComboBoxItem { Content = "📄 Tra cứu", Tag = "Lookup" });
                    FeatureComboBox.Items.Add(new ComboBoxItem { Content = "📝 Báo cáo vi phạm", Tag = "Report" });
                    FeatureComboBox.Items.Add(new ComboBoxItem { Content = "🚗 Cập nhật xe", Tag = "UpdateCar" });
                    FeatureComboBox.Items.Add(new ComboBoxItem { Content = "🚗 Xem báo cáo", Tag = "ReportList" });
                }
                else if (role == "Police" || role == "Admin")
                {
                    FeatureComboBox.Items.Add(new ComboBoxItem { Content = "📄 Tra cứu", Tag = "Lookup" });
                }
            }
            else
            {
                WelcomeText.Text = "Xin chào, khách!";
                btnLogout.Visibility = Visibility.Collapsed;
                FeatureComboBox.Visibility = Visibility.Collapsed;
            }
        }

        private void FeatureComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FeatureComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string tag = selectedItem.Tag?.ToString();

                switch (tag)
                {
                    case "Lookup":
                        OpenLookup(null, null);
                        break;
                    case "Report":
                        OpenReport(null, null);
                        break;
                    case "UpdateCar":
                        OpenUpdateCar(null, null);
                        break;
                    case "ReportList":
                        ReportList(null, null); 
                        break;


                }

                // Reset ComboBox để không giữ lựa chọn
                FeatureComboBox.SelectedIndex = -1;
            }
        }

        private void OpenHome(object sender, RoutedEventArgs e)
        {
            // Đang ở trang chủ, không làm gì cả
        }

        private void OpenRegister(object sender, RoutedEventArgs e)
        {
            new RegisterPage().Show();
            this.Close();
        }

        private void OpenLogin(object sender, RoutedEventArgs e)
        {
            new Login().Show();
            this.Close();
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            UserSession.Logout();
            MessageBox.Show("Bạn đã đăng xuất.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            new Login().Show();
            this.Close();
        }
        private void OpenReport(object sender, RoutedEventArgs e)
        {
            new Report().Show();
            this.Close();
        }
        private void OpenUpdateCar(object sender, RoutedEventArgs e)
        {
            AddVehicle av = new AddVehicle();
            av.ShowDialog();
        }
        private void OpenLookup(object sender, RoutedEventArgs e)
        {

        }
        private void ViewProfile(object sender, RoutedEventArgs e)
        {
            new ViewProfile().Show();
            this.Close();
        }

        private void ReportList(object sender, RoutedEventArgs e)
        {
            new ReportList().ShowDialog();
            //this.Close();
        }
    }
}
