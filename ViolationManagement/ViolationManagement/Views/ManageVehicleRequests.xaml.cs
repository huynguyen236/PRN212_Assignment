using Microsoft.EntityFrameworkCore;
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
using ViolationManagement.Helper;
using ViolationManagement.Models;

namespace ViolationManagement.Views
{
    /// <summary>
    /// Interaction logic for ManageVehicleRequests.xaml
    /// </summary>
    public partial class ManageVehicleRequests : Window
    {
        private int currentPage = 1;
        private int pageSize = 10;
        private int totalPages = 1;
        private readonly ViolationManagementContext _context = new ViolationManagementContext();
        public ManageVehicleRequests()
        {
            InitializeComponent();
            LoadRequests();
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
        private async void ApproveRequest_Click(object sender, RoutedEventArgs e)
        {

            var SelectedRequest = (sender as FrameworkElement)?.DataContext as Models.VehicleAddRequest;
            if (SelectedRequest == null) return;

            var result = MessageBox.Show("Bạn có chắc chắn muốn phê duyệt yêu cầu thêm xe này?",
                                          "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                // Lấy thông tin người dùng hiện tại (người duyệt)
                int currentUserId = int.Parse(UserSession.CurrentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                using (var context = new ViolationManagementContext())
                {
                    // Lấy lại VehicleAddRequest từ DB để đảm bảo trạng thái mới nhất
                    var request = await context.VehicleAddRequests
                        .Include(r => r.Owner)
                        .FirstOrDefaultAsync(r => r.RequestId == SelectedRequest.RequestId);

                    if (request == null || request.Status == "Approved")
                    {
                        MessageBox.Show("Yêu cầu không hợp lệ hoặc đã được phê duyệt.");
                        return;
                    }

                    // Tạo Vehicle mới
                    var newVehicle = new Vehicle
                    {
                        PlateNumber = request.PlateNumber,
                        OwnerId = request.OwnerId,
                        Brand = request.Brand,
                        Model = request.Model,
                        ManufactureYear = request.ManufactureYear
                    };
                    context.Vehicles.Add(newVehicle);

                    // Cập nhật trạng thái yêu cầu
                    request.Status = "Approved";
                    request.ApprovedBy = currentUserId;
                    request.ApprovalDate = DateTime.Now;

                    // Gửi thông báo cho chủ xe
                    context.Notifications.Add(new Notification
                    {
                        UserId = request.OwnerId,
                        PlateNumber = request.PlateNumber,
                        Message = $"Xe có biển số {request.PlateNumber} đã được phê duyệt.",
                        SentDate = DateTime.Now,
                        IsRead = false
                    });

                    context.SaveChanges();
                    MessageBox.Show("Đã phê duyệt và thêm phương tiện thành công.");
                }

                // Làm mới danh sách
                LoadRequests();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}");
            }
        }
        private async void RejectRequest_Click(object sender, RoutedEventArgs e)
        {
            var selectedRequest = (sender as FrameworkElement)?.DataContext as VehicleAddRequest;
            if (selectedRequest == null) return;

            var result = MessageBox.Show("Bạn có chắc chắn muốn từ chối yêu cầu thêm xe này?",
                                         "Xác nhận từ chối", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                int currentUserId = int.Parse(UserSession.CurrentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                using (var context = new ViolationManagementContext())
                {
                    var request = await context.VehicleAddRequests
                        .Include(r => r.Owner)
                        .FirstOrDefaultAsync(r => r.RequestId == selectedRequest.RequestId);

                    if (request == null || request.Status != "Pending")
                    {
                        MessageBox.Show("Yêu cầu không hợp lệ hoặc đã xử lý.");
                        return;
                    }

                    request.Status = "Rejected";
                    request.ApprovedBy = currentUserId;
                    request.ApprovalDate = DateTime.Now;

                    context.Notifications.Add(new Notification
                    {
                        UserId = request.OwnerId,
                        PlateNumber = request.PlateNumber,
                        Message = $"Yêu cầu thêm xe biển số {request.PlateNumber} đã bị từ chối.",
                        SentDate = DateTime.Now,
                        IsRead = false
                    });

                    await context.SaveChangesAsync();
                }

                MessageBox.Show("Đã từ chối yêu cầu thành công.");
                LoadRequests();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void LoadRequests()
        {
            using (var context = new ViolationManagementContext())
            {
                var query = context.VehicleAddRequests
                    .Include(r => r.Owner)
                    .OrderByDescending(r => r.RequestDate)
                    .AsQueryable();

                // Lọc theo trạng thái
                string selectedStatus = (cbStatusFilter.SelectedItem as ComboBoxItem)?.Content?.ToString();
                if (!string.IsNullOrEmpty(selectedStatus) && selectedStatus != "Tất cả")
                {
                    query = query.Where(r => r.Status == selectedStatus);
                }

                // Tìm kiếm theo tên chủ xe
                string keyword = txtSearch.Text.Trim().ToLower();
                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(r => r.Owner.FullName.ToLower().Contains(keyword));
                }

                // Tổng số trang
                int totalItems = query.Count();
                totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

                // Giới hạn theo phân trang
                var pagedList = query
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                dgRequests.ItemsSource = pagedList;
                lblPageInfo.Text = $"Trang {currentPage}/{totalPages}";
            }
        }
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;
            LoadRequests();
        }
        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadRequests();
            }
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadRequests();
            }
        }

    }
}
