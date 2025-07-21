using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using ViolationManagement.Helper;
using ViolationManagement.Models;

namespace ViolationManagement.Controllers
{
    public class NotificationController
    {
        private readonly ViolationManagementContext _context = new();

        // Lấy tất cả thông báo theo UserID
        public List<Notification> GetNotificationsByUserId(string userId)
        {
            return _context.Notifications
                .Where(n => n.UserId.ToString() == userId)
                .OrderByDescending(n => n.SentDate)
                .ToList();
        }

        // Tìm kiếm thông báo theo từ khóa (Message hoặc Biển số)
        public List<Notification> SearchNotifications(string userId, string keyword)
        {
            keyword = keyword.ToLower().Trim();

            return _context.Notifications
                .Where(n => n.UserId.ToString() == userId &&
                            (n.Message.ToLower().Contains(keyword) ||
                             (n.PlateNumber != null && n.PlateNumber.ToLower().Contains(keyword))))
                .OrderByDescending(n => n.SentDate)
                .ToList();
        }

        // Lọc thông báo đã đọc
        public List<Notification> GetReadNotifications(string userId)
        {
            return _context.Notifications
                .Where(n => n.UserId.ToString() == userId && n.IsRead)
                .OrderByDescending(n => n.SentDate)
                .ToList();
        }

        // Lọc thông báo chưa đọc
        public List<Notification> GetUnreadNotifications(string userId)
        {
            return _context.Notifications
                .Where(n => n.UserId.ToString() == userId && !n.IsRead)
                .OrderByDescending(n => n.SentDate)
                .ToList();
        }

        // Đánh dấu 1 thông báo là đã đọc
        public void MarkAsRead(int notificationId)
        {
            var notification = _context.Notifications.FirstOrDefault(n => n.NotificationId == notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                _context.SaveChanges();
            }
        }

        // Đánh dấu tất cả thông báo là đã đọc
        public void MarkAllAsRead(string userId)
        {
            var notifications = _context.Notifications
                .Where(n => n.UserId.ToString() == userId && !n.IsRead)
                .ToList();

            foreach (var n in notifications)
            {
                n.IsRead = true;
            }

            _context.SaveChanges();
        }

        // Xóa thông báo
        public void DeleteNotification(int notificationId)
        {
            var notification = _context.Notifications.FirstOrDefault(n => n.NotificationId == notificationId);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                _context.SaveChanges();
            }
        }

        // Lấy thông báo theo ID
        public Notification? GetNotificationById(int notificationId)
        {
            return _context.Notifications.FirstOrDefault(n => n.NotificationId == notificationId);
        }
    }
}
