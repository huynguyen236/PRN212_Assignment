using System.Windows;

namespace ViolationManagement.Views
{
    public partial class VerificationWindow : Window
    {
        private readonly string _expectedCode;

        public bool IsVerified { get; private set; } = false;

        public VerificationWindow(string code)
        {
            InitializeComponent();
            _expectedCode = code;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (CodeBox.Text.Trim() == _expectedCode)
            {
                IsVerified = true;
                this.DialogResult = true; // Đóng cửa sổ và return true
            }
            else
            {
                ErrorText.Text = "Mã xác minh không đúng. Vui lòng thử lại.";
            }
        }
    }
}
