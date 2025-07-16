using System.Linq;
using System.Windows;
using ViolationManagement.Models;
using ViolationManagement.Helper;
using ViolationManagement.ViewModels;
namespace ViolationManagement.Views
{
    public partial class ViewProfile : Window
    {
        private readonly ViolationManagementContext _context = new();
        private User? _user;
        private ProfileViewModel? _model;
        public ViewProfile()
        {
            InitializeComponent();
            LoadProfile();
        }

        private void LoadProfile()
        {
            int _userId = int.Parse(UserSession.GetClaim(System.Security.Claims.ClaimTypes.NameIdentifier));
            _user = _context.Users.FirstOrDefault(u => u.UserId == _userId);

            if (_user == null)
            {
                MessageBox.Show("Người dùng không tồn tại.");
                this.Close();
                return;
            }

            var vehicles = _context.Vehicles
                .Where(v => v.OwnerId == _userId)
                .Select(v => new Vehicle
                {
                    PlateNumber = v.PlateNumber,
                    Brand = v.Brand,
                    Model = v.Model,
                    ManufactureYear = v.ManufactureYear
                })
                .ToList();

            _model = new ProfileViewModel
            {
                FullName = _user.FullName,
                CitizenID = _user.CitizenId,
                Email = _user.Email,
                Phone = _user.Phone,
                Address = _user.Address,
                Gender = _user.Gender,
                Vehicles = vehicles
            };

            this.DataContext = _model;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (_user == null) return;

            // Cập nhật dữ liệu vào entity
            _user.FullName = _model.FullName;
            _user.Phone = _model.Phone;
            _user.Address = _model.Address;
            _user.Gender = _model.Gender;

            try
            {
                _context.SaveChanges();
                MessageBox.Show("Đã lưu thông tin thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            new HomePage().Show();
            this.Close();
        }
    }
}
