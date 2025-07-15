using System.Windows;

namespace ViolationManagement.Views
{
    public partial class NewPasswordWindow : Window
    {
        private readonly string _expectedCode;

        public string? newPassword { get; private set; } = null;

        public NewPasswordWindow(string code)
        {
            InitializeComponent();
            _expectedCode = code;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            string code = CodeBox.Text.Trim();
            string pass = newpasswordtxt.Password;
            string repass = repasswordtxt.Password;

            if (code != _expectedCode)
            {
                ErrorText.Text = "Mã xác minh không đúng. Vui lòng thử lại.";

            }
            else if (pass != repass)
            {
                ErrorText.Text = "Mật khẩu không khớp. Vui lòng thử lại.";

            }
            else
            {
                newPassword = pass;
                this.DialogResult = true; // Đóng cửa sổ và return true
            }

        }
    }
}
