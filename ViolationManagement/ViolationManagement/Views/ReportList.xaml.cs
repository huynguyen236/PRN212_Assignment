using System.Windows;
using ViolationManagement.Controllers;
using ViolationManagement.Models;
using System.Collections.Generic;
using ViolationManagement.Helper;
using System.Windows.Controls;
using ViolationManagement.ViewModels;

namespace ViolationManagement.Views
{
    public partial class ReportList : Window
    {
        private List<Models.Report> allReports = new();
        private int currentPage = 1;
        private int pageSize = 10;
        private int totalPages = 1;
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

            totalPages = (int)Math.Ceiling((double)allReports.Count / pageSize);
            currentPage = 1;

            LoadPage(currentPage);

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
        private void LoadPage(int page)
        {
            string? userId = UserSession.GetClaim(System.Security.Claims.ClaimTypes.NameIdentifier);

            // Lấy tất cả report của user
            allReports = _controller.GetReportsByUserId(userId);

            // Tính tổng số trang
            totalPages = (int)Math.Ceiling((double)allReports.Count / pageSize);
            if (totalPages == 0) totalPages = 1;

            // Giữ currentPage hợp lệ
            currentPage = Math.Max(1, Math.Min(page, totalPages));

            // Lấy danh sách trong trang hiện tại
            var pagedReports = allReports
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            dgReports.ItemsSource = pagedReports;
            txtPageInfo.Text = $"Trang {currentPage}/{totalPages}";
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadPage(currentPage);
            }
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadPage(currentPage);
            }
        }

    }
}
