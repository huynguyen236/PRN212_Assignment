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
                    FeatureComboBox.Items.Add(new ComboBoxItem { Content = "Thông báo của tôi", Tag = "NotificationList" });  // MỚI

                    FeatureComboBox.Items.Add(new ComboBoxItem { Content = "Báo cáo vi phạm", Tag = "Report" });
                    FeatureComboBox.Items.Add(new ComboBoxItem { Content = "Thêm phương tiện", Tag = "UpdateCar" });
                    FeatureComboBox.Items.Add(new ComboBoxItem { Content = "Xem báo cáo của tôi", Tag = "ReportList" });
                    FeatureComboBox.Items.Add(new ComboBoxItem { Content = "Vi phạm của tôi", Tag = "MyViolation" });
                }
                else if (role == "Admin")
                {
                    FeatureComboBox.Items.Add(new ComboBoxItem { Content = "Thông báo của tôi", Tag = "NotificationList" });  // MỚI

                    FeatureComboBox.Items.Add(new ComboBoxItem { Content = "Quản lí tài khoản", Tag = "ManageAccount" });
                    FeatureComboBox.Items.Add(new ComboBoxItem { Content = "Quản lí báo cáo vi phạm", Tag = "ManageReport" });
                    FeatureComboBox.Items.Add(new ComboBoxItem { Content = "Danh sách vi phạm", Tag = "ViolationList" });                   

                }
                else if (role == "Police")
                {
                    FeatureComboBox.Items.Add(new ComboBoxItem { Content = "Thông báo của tôi", Tag = "NotificationList" });  // MỚI
                    FeatureComboBox.Items.Add(new ComboBoxItem { Content = "Quản lí báo cáo vi phạm", Tag = "ManageReport" });
                    FeatureComboBox.Items.Add(new ComboBoxItem { Content = "Quản lí yêu cầu thêm xe", Tag = "ManageVehicleRequests" });
                    FeatureComboBox.Items.Add(new ComboBoxItem { Content = "Danh sách vi phạm", Tag = "ViolationList" });

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
                    case "ManageAccount":
                        ManageAccount(null, null);
                        break;
                    case "ReportList":
                        ReportList(null, null);
                        break;
                    case "ManageReport":
                        ManageReport(null, null);
                        break;
                    case "MyViolation":
                        Myviolation(null, null); 
                        break;
                    case "ManageVehicleRequests":
                        ManageVehicleRequests(null, null);
                        break;
                    case "NotificationList":
                        NotificationList(null, null);
                        break;
                    case "ViolationList":
                        ViolationList(null, null);
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
           new ReportList().Show();
            this.Close();
        }
        private void ManageAccount(object sender, RoutedEventArgs e)
        {
            new ManageAccount().Show();
            this.Close();
        }
        private void ManageReport(object sender, RoutedEventArgs e)
        {
            new ManageReports().Show();
            this.Close();
        }


        private void Myviolation(object sender, RoutedEventArgs e)
        {
            new MyViolation().Show();
            this.Close();
        }

        private void ManageVehicleRequests(object sender, RoutedEventArgs e)
        {
            new ManageVehicleRequests().Show();

            this.Close();
        }

        private void NotificationList(object sender, RoutedEventArgs e)
        {
            new NotificationList().Show();

            this.Close();
        }
        private void ViolationList(object sender, RoutedEventArgs e)
        {
            new ViolationList().Show();

            this.Close();
        }
        
    }

}

