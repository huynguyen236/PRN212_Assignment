using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using ViolationManagement.Controllers;
using ViolationManagement.Helper;

namespace ViolationManagement.Views
{
    public partial class Report : Window
    {
        private readonly ReportController _controller = new();

        private string _selectedImagePath = "";
        private string _selectedVideoPath = "";

        public Report()
        {
            InitializeComponent();
        }

        private void SelectImage(object sender, RoutedEventArgs e)
        {
            string? selectedPath = _controller.SelectFile("Chọn ảnh", "Image files (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp");

            if (selectedPath != null)
            {
                string extension = System.IO.Path.GetExtension(selectedPath).ToLower();
                var allowedExtensions = new[] { ".png", ".jpg", ".jpeg", ".bmp" };

                if (!allowedExtensions.Contains(extension))
                {
                    MessageBox.Show("Vui lòng chọn ảnh có định dạng .png, .jpg, .jpeg hoặc .bmp.", "Lỗi định dạng", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                long fileSize = new FileInfo(selectedPath).Length;
                if (fileSize > 5 * 1024 * 1024) // > 5MB
                {
                    MessageBox.Show("Ảnh vượt quá dung lượng cho phép (tối đa 5MB).", "Lỗi dung lượng", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _selectedImagePath = selectedPath;
                ImagePathBox.Text = selectedPath;
            }
        }

        private void SelectVideo(object sender, RoutedEventArgs e)
        {
            string? selectedPath = _controller.SelectFile("Chọn video", "Video files (*.mp4;*.avi;*.mov)|*.mp4;*.avi;*.mov");

            if (selectedPath != null)
            {
                string extension = System.IO.Path.GetExtension(selectedPath).ToLower();
                var allowedExtensions = new[] { ".mp4", ".avi", ".mov" };

                if (!allowedExtensions.Contains(extension))
                {
                    MessageBox.Show("Vui lòng chọn video có định dạng .mp4, .avi hoặc .mov.", "Lỗi định dạng", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                long fileSize = new FileInfo(selectedPath).Length;
                if (fileSize > 10 * 1024 * 1024) // > 100MB
                {
                    MessageBox.Show("Video vượt quá dung lượng cho phép (tối đa 100MB).", "Lỗi dung lượng", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _selectedVideoPath = selectedPath;
                VideoPathBox.Text = selectedPath;
            }
        }


        private void SubmitReport(object sender, RoutedEventArgs e)
        {
            string violationType = ViolationTypeTextBox.Text.Trim();
            string description = DescriptionTextBox.Text.Trim();
            string plate = PlateNumberTextBox.Text.Trim();
            string location = LocationTextBox.Text.Trim();

            bool success = _controller.SubmitReport(
                violationType, description, plate, location,
                _selectedImagePath, _selectedVideoPath, out string message
            );

            if (success)
            {
                MessageBox.Show("Phản ánh đã được gửi!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                // Reset form nếu cần
            }
            else
            {
                MessageBox.Show(message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void OpenHome(object sender, RoutedEventArgs e)
        {
            HomePage hp = new HomePage();
            hp.Show();
            this.Close();
        }
        private void OpenLookup(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng đang được phát triển.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OpenReport(object sender, RoutedEventArgs e)
        {
            
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            UserSession.Logout();

            MessageBox.Show("Bạn đã đăng xuất.", "Đăng xuất", MessageBoxButton.OK, MessageBoxImage.Information);

            var login = new Login();
            login.Show();
            this.Close();
        }
       private void OpenUpdateCar(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng đang được phát triển.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }
       
    }
}
