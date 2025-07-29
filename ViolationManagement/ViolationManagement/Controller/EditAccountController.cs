using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using ViolationManagement.Models;
using ViolationManagement.Views;

namespace ViolationManagement.Controller
{
    public class EditAccountController
    {
        private readonly ViolationManagementContext _context = new ViolationManagementContext();
        public bool EditAccount(string cccd, string email, string phone, string fullName, string gender, string address, string role, out string message)
        {
            message = "";

            // Validate trống
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(cccd) ||
                string.IsNullOrWhiteSpace(gender) || string.IsNullOrWhiteSpace(address) ||
                string.IsNullOrWhiteSpace(role))
            {
                message = "Vui lòng nhập đầy đủ thông tin.";
                return false;
            }

            // Validate email
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                message = "Email không hợp lệ.";
                return false;
            }

            // Validate số điện thoại
            if (!Regex.IsMatch(phone, @"^0\d{9}$"))
            {
                message = "Số điện thoại phải bắt đầu bằng số 0 và gồm 10 chữ số.";
                return false;
            }

            // Tìm user theo CCCD
            var user = _context.Users.FirstOrDefault(u => u.CitizenId == cccd);
            if (user == null)
            {
                message = "Không tìm thấy người dùng.";
                return false;
            }

            // Kiểm tra email trùng (với người khác)
            if (_context.Users.Any(u => u.Email == email && u.UserId != user.UserId))
            {
                message = "Email đã tồn tại. Vui lòng dùng email khác.";
                return false;
            }

            // Kiểm tra số điện thoại trùng (với người khác)
            if (_context.Users.Any(u => u.Phone == phone && u.UserId != user.UserId))
            {
                message = "Số điện thoại đã tồn tại. Vui lòng dùng số khác.";
                return false;
            }

            // Gán thông tin mới
            user.FullName = fullName;
            user.Email = email;
            user.Phone = phone;
            user.Address = address;
            user.Gender = gender;
            user.Role = role;

            _context.SaveChanges();
            return true;
        }

    }
}
