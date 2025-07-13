
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using ViolationManagement.Models;

namespace ViolationManagement.Controllers
{
    public class RegisterController
    {
        private readonly ViolationManagementContext _context = new();

        public bool Register(string email, string phone, string fullName, string password, string confirmPassword, out string message)
        {
            message = "";

            // Validate trống
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                message = "Vui lòng nhập đầy đủ thông tin.";
                return false;
            }

            // Validate email định dạng
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                message = "Email không hợp lệ.";
                return false;
            }

            // Check email trùng
            if (_context.Users.Any(u => u.Email == email))
            {
                message = "Email đã tồn tại. Vui lòng dùng email khác.";
                return false;
            }

            // Validate phone
            if (!Regex.IsMatch(phone, @"^0\d{9}$"))
            {
                message = "Số điện thoại phải bắt đầu bằng số 0 và gồm 10 chữ số.";
                return false;
            }

            // Check password khớp
            if (password != confirmPassword)
            {
                message = "Mật khẩu xác nhận không khớp.";
                return false;
            }

            // Tạo đối tượng User mới
            var newUser = new User
            {
                FullName = fullName,
                Email = email,
                Phone = phone,
                Password = password,
                Role = "Citizen"
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return true;
        }
    }
}
