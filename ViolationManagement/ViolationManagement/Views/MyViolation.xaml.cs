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
using ViolationManagement.Models;
using ViolationManagement.ViewModels;

namespace ViolationManagement.Views
{
    /// <summary>
    /// Interaction logic for MyViolation.xaml
    /// </summary>
    public partial class MyViolation : Window
    {
        private List<MyViolationViewModel> allReports = new();
        private int currentPage = 1;
        private int pageSize = 10;
        private int totalPages = 1;


        private readonly MyViolationController _controller = new();
        public MyViolation()
        {
            InitializeComponent();
            LoadReports();
            currentPage = 1;
            ApplyPaging();
        }
      

        private void LoadReports()
        {
            string? userId = UserSession.GetClaim(ClaimTypes.NameIdentifier);
            allReports = _controller.GetReportsByUserId(userId);
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



        private void ApplyPaging()
        {
            var selectedItem = cbStatusFilter.SelectedItem as ComboBoxItem;
            var selectedTag = selectedItem?.Tag?.ToString();

            var filteredReports = allReports;

            if (selectedTag != "All")
            {
                string selectedText = selectedItem.Content.ToString();
                filteredReports = allReports
                    .Where(v => string.Equals(v.PaidStatusText, selectedText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            totalPages = (int)Math.Ceiling((double)filteredReports.Count / pageSize);
            if (totalPages == 0) totalPages = 1;
            if (currentPage > totalPages) currentPage = totalPages;

            var pagedReports = filteredReports
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            dgViolations.ItemsSource = pagedReports;
            txtPageInfo.Text = $"Trang {currentPage} / {totalPages}";
        }
        private void FirstPage_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;
            ApplyPaging();
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                ApplyPaging();
            }
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                ApplyPaging();
            }
        }

        private void LastPage_Click(object sender, RoutedEventArgs e)
        {
            currentPage = totalPages;
            ApplyPaging();
        }

    }
}
    