using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using ViolationManagement.Controllers;
using ViolationManagement.Helper;

namespace ViolationManagement.Views
{
    /// <summary>
    /// Interaction logic for AddVehicle.xaml
    /// </summary>
    public partial class AddVehicle : Window
    {
        private VehicleController _controller;
        public AddVehicle()
        {
            InitializeComponent();
            _controller = new VehicleController();  
        }
        private void BtnAddVehicle_Click(object sender, RoutedEventArgs e)
        {
            string plate = txtPlate.Text.Trim();
            string brand = txtBrand.Text.Trim();
            string model = txtModel.Text.Trim();
            string yearText = txtYear.Text.Trim();

            // Kiểm tra đăng nhập
            string? userId = UserSession.GetClaim(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (!int.TryParse(userId, out int ownerId))
            {
                MessageBox.Show("Không xác định được người dùng.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Kiểm tra các trường rỗng
            if (string.IsNullOrEmpty(plate) || string.IsNullOrEmpty(brand) || string.IsNullOrEmpty(model) || string.IsNullOrEmpty(yearText))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tất cả các trường.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Kiểm tra năm sản xuất
            if (!int.TryParse(yearText, out int year) || year < 1900 || year > DateTime.Now.Year)
            {
                MessageBox.Show($"Năm sản xuất không hợp lệ. Vui lòng nhập số từ 1900 đến {DateTime.Now.Year}.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Gửi yêu cầu thêm phương tiện
            bool success = _controller.SubmitVehicleRequest(plate, brand, model, year, ownerId, out string message);

            if (success)
            {
                MessageBox.Show("Gửi yêu cầu thêm phương tiện thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show(message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
