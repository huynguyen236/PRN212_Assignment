using System;
using System.Linq;
using System.Windows;
using ViolationManagement.Controller;
using ViolationManagement.Models;

namespace ViolationManagement.Views
{
    public partial class ForgotPassword : Window
    {
        private readonly ViolationManagementContext _context = new();
        private readonly ForgotPasswordController _controller = new();

        public ForgotPassword()
        {
            InitializeComponent();
        }

        private void SendNewPassword_Click(object sender, RoutedEventArgs e)
        {
            string cccd = CCCDTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(cccd))
            {
                ResultTextBlock.Text = "Vui lòng nhập mã CCCD.";
                return;
            }

            var user = _context.Users.FirstOrDefault(u => u.CitizenId == cccd);

            if (user == null)
            {
                ResultTextBlock.Text = "Mã CCCD không tồn tại trong hệ thống.";
                return;
            }


            bool success = _controller.ForgotPassword(cccd, out string message);

            if (success)
            {
                MessageBox.Show(message, "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show(message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
            }


        }
    }
}
