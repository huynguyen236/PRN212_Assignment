using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ViolationManagement.Models;
using ViolationManagement.Helper;

namespace ViolationManagement.Views
{
    public partial class ViolationList : Window
    {
        private ViolationManagementContext _context;

        public ViolationList()
        {
            InitializeComponent();
            _context = new ViolationManagementContext();
            LoadData();
        }

        private void LoadData()
        {
            var data = (from v in _context.Violations
                        join r in _context.Reports on v.ReportId equals r.ReportId
                        select new
                        {
                            v.ViolationId,
                            v.PlateNumber,
                            r.ViolationType,
                            r.Description,
                            r.Location,
                            v.FineDate,
                            v.FineAmount,
                            v.PaidStatus,
                            PaidStatusText = v.PaidStatus ? "Đã thanh toán" : "Chưa thanh toán",
                            v.ViolatorId
                        }).ToList();

            dgViolations.ItemsSource = data;
        }

        private void ConfirmPayment_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = ((FrameworkElement)sender).DataContext;

            var prop = selectedItem.GetType().GetProperty("ViolationId");
            int violationId = (int)prop.GetValue(selectedItem);

            var violatorIdProp = selectedItem.GetType().GetProperty("ViolatorId");
            int violatorId = (int)violatorIdProp.GetValue(selectedItem);

            var plateNumberProp = selectedItem.GetType().GetProperty("PlateNumber");
            string plateNumber = (string)plateNumberProp.GetValue(selectedItem);

            var violation = _context.Violations.FirstOrDefault(v => v.ViolationId == violationId);
            if (violation != null)
            {
                violation.PaidStatus = true;

                // Gửi thông báo
                if (violatorId != 0)
                {
                    var noti = new Notification
                    {
                        UserId = violatorId,
                        Message = $"Vi phạm với biển số {plateNumber} đã được thanh toán.",
                        PlateNumber = plateNumber,
                        SentDate = DateTime.Now,
                        IsRead = false
                    };
                    _context.Notifications.Add(noti);
                }

                _context.SaveChanges();

                MessageBox.Show("Đã xác nhận thanh toán!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadData();
            }
        }

        private void OpenHome(object sender, RoutedEventArgs e)
        {
            new HomePage().Show();
            this.Close();
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            UserSession.Logout();
            new Login().Show();
            this.Close();
        }
    }
}
