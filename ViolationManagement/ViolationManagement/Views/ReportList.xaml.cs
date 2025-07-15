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
                    : _controller.GetReportsByStatus(userId, selectedStatus);

                dgReports.ItemsSource = reports;
            }
        }

    }
}
