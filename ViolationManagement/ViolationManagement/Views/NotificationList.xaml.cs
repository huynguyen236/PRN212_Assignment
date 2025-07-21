using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ViolationManagement.Controllers;
using ViolationManagement.Helper;
using ViolationManagement.Models;

namespace ViolationManagement.Views
{
    public partial class NotificationList : Window
    {
        private NotificationController _controller;
        private string? lastFilter = "All"; // Nhớ lần lọc cuối

        public NotificationList()
        {
            InitializeComponent();
            _controller = new NotificationController();

            LoadNotifications();
        }

        private void LoadNotifications()
        {
            string? userId = UserSession.GetClaim(System.Security.Claims.ClaimTypes.NameIdentifier);

            var notifications = _controller.GetNotificationsByUserId(userId);
            dgNotifications.ItemsSource = notifications;

            UpdateWindowTitle(notifications);
        }

        private void UpdateWindowTitle(List<Notification> notifications)
        {
            int unreadCount = notifications.Count(n => !n.IsRead);
            this.Title = unreadCount > 0 ? $"Thông báo của bạn ({unreadCount} chưa đọc)" : "Thông báo của bạn";
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            keyword = string.Join(" ", keyword.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

            string? userId = UserSession.GetClaim(System.Security.Claims.ClaimTypes.NameIdentifier);

            var notifications = _controller.SearchNotifications(userId, keyword);

            if (notifications.Count == 0)
            {
                MessageBox.Show("Không tìm thấy thông báo nào với từ khóa này.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            dgNotifications.ItemsSource = notifications;
            UpdateWindowTitle(notifications);
        }

        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {
            if (cmbFilter.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedFilter = selectedItem.Tag?.ToString();
                lastFilter = selectedFilter;

                string? userId = UserSession.GetClaim(System.Security.Claims.ClaimTypes.NameIdentifier);

                List<Notification> notifications;

                if (selectedFilter == "All" || string.IsNullOrEmpty(selectedFilter))
                {
                    notifications = _controller.GetNotificationsByUserId(userId);
                }
                else if (selectedFilter == "Read")
                {
                    notifications = _controller.GetReadNotifications(userId);
                }
                else // Unread
                {
                    notifications = _controller.GetUnreadNotifications(userId);
                }

                if (notifications.Count == 0)
                {
                    MessageBox.Show("Không có thông báo nào phù hợp với bộ lọc.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                dgNotifications.ItemsSource = notifications;
                UpdateWindowTitle(notifications);
            }
        }

        private void BtnDetail_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Notification notification)
            {
                MessageBox.Show(notification.Message, "Chi tiết thông báo");

                if (!notification.IsRead)
                {
                    _controller.MarkAsRead(notification.NotificationId);
                    ReloadWithLastFilter();
                }
            }
            else
            {
                MessageBox.Show("Không lấy được thông tin thông báo", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Notification notification)
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa thông báo này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    _controller.DeleteNotification(notification.NotificationId);
                    ReloadWithLastFilter();
                }
            }
            else
            {
                MessageBox.Show("Không lấy được thông tin thông báo", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnMarkAllRead_Click(object sender, RoutedEventArgs e)
        {
            string? userId = UserSession.GetClaim(System.Security.Claims.ClaimTypes.NameIdentifier);

            _controller.MarkAllAsRead(userId);
            ReloadWithLastFilter();
        }

        private void ReloadWithLastFilter()
        {
            // Giữ trạng thái filter sau khi hành động
            if (lastFilter == "Read" || lastFilter == "Unread" || lastFilter == "All")
            {
                BtnFilter_Click(null, null);
            }
            else
            {
                LoadNotifications();
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
