using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using ViolationManagement.Models;
using ViolationManagement.Views;
using ViolationManagement.Controllers;
using Microsoft.IdentityModel.Tokens;

namespace ViolationManagement.Controller
{
    public class ForgotPasswordController
    {
        private readonly ViolationManagementContext _context = new();

        public bool ForgotPassword(string cccd, out string message)
        {
            message = "";

            var user = _context.Users.FirstOrDefault(u => u.CitizenId == cccd);
            


            // Tạo mã xác nhận
            var code = new Random().Next(100000, 999999).ToString();

            // Gửi email
            bool sent = SendVerificationEmail(user.Email, code, out string error);

            if (!sent)
            {
                message = "Gửi email thất bại: " + error;
                return false;
            }
            else
            {
                var newpasswordwindow = new NewPasswordWindow(code);
                bool? result = newpasswordwindow.ShowDialog();

                if (result.HasValue && result == true && !newpasswordwindow.newPassword.IsNullOrEmpty())
                {
                    user.Password = newpasswordwindow.newPassword;
                    _context.SaveChanges();
                    message = "Đổi mật khẩu thành công!";
                    return true;
                }
                message = "Đổi mật khẩu thất bại!";

                return false;
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
                    Subject = "Mã xác minh tài khoản",
                    Body = $"Xin chào quý khách,\n\nMã xác minh của bạn là: {code}\n\nVui lòng nhập mã này để hoàn tất việc đổi mật khẩu.",
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
