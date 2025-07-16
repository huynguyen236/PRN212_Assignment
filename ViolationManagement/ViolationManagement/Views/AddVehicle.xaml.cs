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
        private bool IsValidPlateNumber(string plateNumber)
        {
            if (string.IsNullOrWhiteSpace(plateNumber))
                return false;

            plateNumber = plateNumber.Trim().ToUpper();

            if (plateNumber.Contains('.'))
                return false;

            string pattern = @"^(?:[0-9]{2}[A-Z]{1}|[0-9]{2}[A-Z]{2})-[0-9]{5}$";
            return Regex.IsMatch(plateNumber, pattern);
        }
        private void BtnAddVehicle_Click(object sender, RoutedEventArgs e)
        {
            string plate = txtPlate.Text.Trim();
            string brand = txtBrand.Text.Trim();
            string model = txtModel.Text.Trim();
            string yearText = txtYear.Text.Trim();

            if (string.IsNullOrEmpty(plate) || !int.TryParse(yearText, out int year))
            {
                MessageBox.Show("Nhập đầy đủ các trường", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (string.IsNullOrEmpty(plate) || string.IsNullOrEmpty(brand) || string.IsNullOrEmpty(model) || string.IsNullOrEmpty(yearText))
            {
                MessageBox.Show("Nhập đầy đủ tất cả các trường.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(yearText, out year) || year < 1900 || year > DateTime.Now.Year)
            {
                MessageBox.Show("Năm sản xuất không hợp lệ. Phải là số từ 1900 đến hiện tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string? userId = UserSession.GetClaim(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (!int.TryParse(userId, out int ownerId))
            {
                MessageBox.Show("Không xác định được người dùng", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            

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
