using System;
using System.Collections.Generic;
using System.Linq;
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
using ViolationManagement.DAL;
using ViolationManagement.Helper;
using ViolationManagement.Models;

namespace ViolationManagement.Views
{
    /// <summary>
    /// Interaction logic for ManageReports.xaml
    /// </summary>
    public partial class ManageReports : Window
    {
        private int currentPage = 1;
        private int pageSize = 10;
        private int totalCount = 0;
        private readonly ReportDAL _reportDAL = new ReportDAL(new ViolationManagementContext());
        public ManageReports()
        {
            InitializeComponent();
            this.Loaded += Window_Loaded;
        }
       

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadReports();
        }

        private void LoadReports()
        {
            string keyword = txtSearch.Text.Trim();

            var reports = _reportDAL.SearchReports(keyword, currentPage, pageSize, out totalCount);
            dgReports.ItemsSource = reports;

            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            lblPageInfo.Text = $"Trang {currentPage}/{Math.Max(totalPages, 1)}";
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;
            LoadReports();
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadReports();
            }
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadReports();
            }
        }

        private void ApproveReport_Click(object sender, RoutedEventArgs e)
        {
            var report = (sender as FrameworkElement)?.DataContext as Models.Report; // Changed Report to ReportDto
            if (report != null)
            {
                _reportDAL.ApproveReport(report.ReportId); // ReportId is accessible in ReportDto
                LoadReports();
            }
        }

        private void RejectReport_Click(object sender, RoutedEventArgs e)
        {
            var report = (sender as FrameworkElement)?.DataContext as Models.Report; // Changed Report to ReportDto
            if (report != null)
            {
                _reportDAL.RejectReport(report.ReportId); // ReportId is accessible in ReportDto
                LoadReports();
            }
        }



        private void dgReports_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            var report = dgReports.SelectedItem as Models.Report;
            if (report != null)
            {
                var detailWindow = new ReportDetailWindow(report.ReportId);
                detailWindow.ShowDialog();
            }
        }
        private void OpenHome(object sender, RoutedEventArgs e)
        {
            HomePage hp = new HomePage();
            hp.Show();
            this.Close();
        }
        private void OpenLookup(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng đang được phát triển.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void Logout(object sender, RoutedEventArgs e)
        {
            UserSession.Logout();

            MessageBox.Show("Bạn đã đăng xuất.", "Đăng xuất", MessageBoxButton.OK, MessageBoxImage.Information);

            var login = new Login();
            login.Show();
            this.Close();
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
    }
}
