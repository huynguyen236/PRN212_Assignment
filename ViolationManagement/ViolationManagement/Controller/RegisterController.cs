
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using ViolationManagement.Models;
using ViolationManagement.Views;

namespace ViolationManagement.Controllers
{
    public class RegisterController
    {
        private readonly ViolationManagementContext _context = new();

        public bool Register(string cccd, string email, string phone, string fullName, string gender, string address, string password, string confirmPassword, out string message)
        {
            message = "";

            // Validate trống
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword) || string.IsNullOrWhiteSpace(cccd) ||
                 string.IsNullOrWhiteSpace(gender) || string.IsNullOrWhiteSpace(address))
            {
                message = "Vui lòng nhập đầy đủ thông tin.";
                return false;
            }

            // Validate CCCD định dạng
            if (string.IsNullOrWhiteSpace(cccd) || !Regex.IsMatch(cccd, @"^0\d{11}$"))
            {
                message = "Mã CCCD phải gồm 12 chữ số và bắt đầu bằng số 0.";
                return false;
            }

            // Check cccd trùng
            if (_context.Users.Any(u => u.CitizenId == cccd))
            {
                message = "Tài khoản của mã CCCD này đã tồn tại.";
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

            if (_context.Users.Any(u => u.Phone == phone))
            {
                message = "Số điện thoại đã tồn tại. Vui lòng dùng số điện thoại khác.";
                return false;
            }

            // Check password khớp
            if (password != confirmPassword)
            {
                message = "Mật khẩu xác nhận không khớp.";
                return false;
            }


            // Tạo mã xác nhận
            var code = new Random().Next(100000, 999999).ToString();

            // Gửi email
            bool sent = SendVerificationEmail(email, code, out string error);

            if (!sent)
            {
                message = "Gửi email thất bại: " + error;
                return false;
            }
            else
            {
                // Tạo đối tượng User mới
                var newUser = new User
                {
                    CitizenId = cccd,
                    FullName = fullName,
                    Email = email,
                    Phone = phone,
                    Gender = gender,
                    Address = address,
                    Password = password,
                    Role = "Citizen"
                };

                var verifyWindow = new VerificationWindow(code);
                bool? result = verifyWindow.ShowDialog();

                if (result.HasValue && result == true)
                {
                    _context.Users.Add(newUser);
                    _context.SaveChanges();
                    message = "Đăng ký thành công!";
                    return true;
                }
                message = "Đăng ký thất bại!";

                return false;
            }


        }

        public bool AddAccount(string cccd, string email, string phone, string fullName, string role, string gender, string address, out string message)
        {
            message = "";

            // Validate trống
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(role) ||
                 string.IsNullOrWhiteSpace(cccd) || string.IsNullOrWhiteSpace(gender) ||
                 string.IsNullOrWhiteSpace(address))
            {
                message = "Vui lòng nhập đầy đủ thông tin.";
                return false;
            }

            // Validate CCCD định dạng
            if (string.IsNullOrWhiteSpace(cccd) || !Regex.IsMatch(cccd, @"^0\d{11}$"))
            {
                message = "Mã CCCD phải gồm 12 chữ số và bắt đầu bằng số 0.";
                return false;
            }

            // Check cccd trùng
            if (_context.Users.Any(u => u.CitizenId == cccd))
            {
                message = "Tài khoản của mã CCCD này đã tồn tại.";
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

            if (_context.Users.Any(u => u.Phone == phone))
            {
                message = "Số điện thoại đã tồn tại. Vui lòng dùng số điện thoại khác.";
                return false;
            }


            // Tạo password ngẫu nhiên
            var password = new Random().Next(100000, 999999).ToString();

            // Gửi email
            bool sent = SendAccountDetailEmail(email, cccd, password, out string error);

            if (!sent)
            {
                message = "Gửi email thất bại: " + error;
                return false;
            }
            else
            {
                // Tạo đối tượng User mới
                var newUser = new User
                {
                    CitizenId = cccd,
                    FullName = fullName,
                    Email = email,
                    Phone = phone,
                    Gender = gender,
                    Address = address,
                    Password = password,
                    Role = role
                };

                _context.Users.Add(newUser);
                _context.SaveChanges();
                message = "Thêm tài khoản thành công";
                return true;
            }


        }

        private bool SendVerificationEmail(string toEmail, string code, out string error)
        {
            try
            {
                var fromEmail = "kdodjeksdkkd@gmail.com";
                var fromPassword = "vuup itfu rwax sckd"; // Phải là App Password của Gmail

                var smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(fromEmail, fromPassword),
                    Timeout = 10000 // 10 giây timeout

                };

                var message = new MailMessage(fromEmail, toEmail)
                {
                    Subject = "Mã xác minh đăng ký tài khoản",
                    Body = $"Xin chào quý khách,\n\nMã xác minh của bạn là: {code}\n\nVui lòng nhập mã này để hoàn tất việc xác thực email.",
                    IsBodyHtml = false
                };

                smtp.Send(message);
                error = "";
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        private bool SendAccountDetailEmail(string toEmail, string cccd, string code, out string error)
        {
            try
            {
                var fromEmail = "kdodjeksdkkd@gmail.com";
                var fromPassword = "vuup itfu rwax sckd"; // Phải là App Password của Gmail

                var smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(fromEmail, fromPassword),
                    Timeout = 10000 // 10 giây timeout

                };

                var message = new MailMessage(fromEmail, toEmail)
                {
                    Subject = "Thông tin tài khoản",
                    Body = $"Xin chào bạn,\n\n Thông tin đăng nhập của bạn là:\n" +
                    $"Số căn cước công dân: {cccd}\n" +
                    $"Mật khẩu: {code}",
                    IsBodyHtml = false
                };

                smtp.Send(message);
                error = "";
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }
    }
}
