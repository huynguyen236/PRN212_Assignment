using System.Windows;
using ViolationManagement.Controllers;
using ViolationManagement.Models;
using System.Collections.Generic;
using ViolationManagement.Helper;
using System.Windows.Controls;

namespace ViolationManagement.Views
{
    public partial class ReportList : Window
    {
        private ReportController _controller;
        public ReportList()
        {
            InitializeComponent();
            _controller = new ReportController();

            LoadReports();
        }

        private void LoadReports()
        {
            string? userId = UserSession.GetClaim(System.Security.Claims.ClaimTypes.NameIdentifier);

            var reports = _controller.GetReportsByUserId(userId); // models.Report
            dgReports.ItemsSource = reports;

        }
        private void BtnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            keyword = string.Join(" ", keyword.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            var reports = _controller.GetReportsByPlate(keyword);

            dgReports.ItemsSource = reports;
        }
        private void BtnLoc_Click(object sender, RoutedEventArgs e)
        {
            if (cmbStatus.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedStatus = selectedItem.Tag?.ToString();
                string? userId = UserSession.GetClaim(System.Security.Claims.ClaimTypes.NameIdentifier);

                var reports = (selectedStatus == "All" || string.IsNullOrEmpty(selectedStatus))
                    ? _controller.GetReportsByUserId(userId)
                    : _controller.GetReportsByStatus(selectedStatus, userId);
                dgReports.ItemsSource = reports;
            }
        }


        private void BtnViewDetail_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Models.Report report)
            {
                var window = new ViewReport(report.ReportId);
                window.ShowDialog();
            }
            else
            {
                MessageBox.Show("Không lấy được thông tin phản ánh");
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

    }
}
