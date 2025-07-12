using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using ViolationManagement.Models;

namespace ViolationManagement
{
    public partial class RegisterPage : Window
    {
        private readonly ViolationManagementContext _context = new ViolationManagementContext();

        public RegisterPage()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text.Trim();
            string phone = PhoneTextBox.Text.Trim();
            string fullName = FullNameTextBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            // Validate trống
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validate email định dạng
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Email không hợp lệ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Check email trùng
            if (_context.Users.Any(u => u.Email == email))
            {
                MessageBox.Show("Email đã tồn tại. Vui lòng dùng email khác.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validate phone
            if (!Regex.IsMatch(phone, @"^0\d{9}$"))
            {
                MessageBox.Show("Số điện thoại phải bắt đầu bằng số 0 và gồm 10 chữ số.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Check password khớp
            if (password != confirmPassword)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Tạo đối tượng User mới
            var newUser = new User
            {
                FullName = fullName,
                Email = email,
                Phone = phone,
                Password = password, // Trong thực tế nên mã hóa mật khẩu
                Role = "Citizen" // Gán mặc định
            };

            // Lưu vào database
            _context.Users.Add(newUser);
            _context.SaveChanges();

            MessageBox.Show("Đăng ký thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

            // Điều hướng sang trang đăng nhập
            var loginWindow = new Login();
            loginWindow.Show();
            this.Close();
        }

        private void OpenHome(object sender, RoutedEventArgs e)
        {
            var home = new HomePage();
            home.Show();
            this.Close();
        }

        private void OpenLookup(object sender, RoutedEventArgs e)
        {
            //var lookup = new LookupPage();
            //lookup.Show();
            //this.Close();
        }

        private void OpenRegister(object sender, RoutedEventArgs e)
        {
            // đang ở trang Register
        }

        private void OpenLogin(object sender, RoutedEventArgs e)
        {
            var login = new Login();
            login.Show();
            this.Close();
        }
    }
}
