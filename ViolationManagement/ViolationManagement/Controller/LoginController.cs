using System.Linq;
using ViolationManagement.Models;

namespace ViolationManagement.Controllers
{
    public class LoginController
    {
        private readonly ViolationManagementContext _context;

        public LoginController()
        {
            _context = new ViolationManagementContext();
        }

        /// <summary>
        /// Kiểm tra thông tin đăng nhập
        /// </summary>
        /// <param name="email">Email người dùng</param>
        /// <param name="password">Mật khẩu người dùng</param>
        /// <returns>Đối tượng User nếu đăng nhập thành công, null nếu thất bại</returns>
        public User Login(string email, string password)
        {
            // Kiểm tra thông tin đăng nhập trong bảng Users
            var user = _context.Users
                .FirstOrDefault(u => u.Email == email && u.Password == password);

            return user;
        }
    }
}
