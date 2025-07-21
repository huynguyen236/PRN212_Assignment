using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
using ViolationManagement.Controller;
using ViolationManagement.Controllers;
using ViolationManagement.Helper;

namespace ViolationManagement.Views
{
    /// <summary>
    /// Interaction logic for MyViolation.xaml
    /// </summary>
    public partial class MyViolation : Window
    {
        private readonly MyViolationController _controller = new();
        public MyViolation()
        {
            InitializeComponent();
            LoadReports();
        }
      

        private void LoadReports()
        {
            string? userId = UserSession.GetClaim(ClaimTypes.NameIdentifier);
            var reports = _controller.GetReportsByUserId(userId);
            dgViolations.ItemsSource = reports;
        }

        public bool PaidStatus { get; set; } // true: Đã thanh toán, false: Chưa thanh toán

        public string PaidStatusText
        {
            get
            {
                return PaidStatus ? "Đã thanh toán" : "Chưa thanh toán";
            }
        }
        private void OpenHome(object sender, RoutedEventArgs e)
        {
            HomePage hp = new HomePage();
            hp.Show();
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
        private void FilterByStatus_Click(object sender, RoutedEventArgs e)
        {
            string? userId = UserSession.GetClaim(ClaimTypes.NameIdentifier);
            var reports = _controller.GetReportsByUserId(userId); // Tải danh sách mới

            var selectedItem = cbStatusFilter.SelectedItem as ComboBoxItem;
            var selectedTag = selectedItem?.Tag?.ToString();

            if (selectedTag != "All")
            {
                reports = reports
                    .Where(v => string.Equals(v.PaidStatusText, selectedItem.Content.ToString(), StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            dgViolations.ItemsSource = reports;
        }

    }
}
    