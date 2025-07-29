using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ViolationManagement.DAL;
using ViolationManagement.Helper;
using ViolationManagement.Models;

namespace ViolationManagement.Views
{
    /// <summary>
    /// Interaction logic for ManageAccount.xaml
    /// </summary>
    public partial class ManageAccount : Window
    {
        private int currentPage = 1;
        private int pageSize = 10;
        private int totalCount = 0;
        private readonly UserDAL _userDAL = new UserDAL(new ViolationManagementContext());
        public ManageAccount()
        {
            InitializeComponent();
            this.Loaded += Window_Loaded;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadRoles();
            LoadUsers();
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
        private void Logout(object sender, RoutedEventArgs e)
        {
            UserSession.Logout();

            MessageBox.Show("Bạn đã đăng xuất.", "Đăng xuất", MessageBoxButton.OK, MessageBoxImage.Information);

            var login = new Login();
            login.Show();
            this.Close();
        }
        private void LoadRoles()
        {
            var roles = _userDAL.GetDistinctRole();
            roles.Insert(0, "Tất cả"); 
            cbRoleFilter.ItemsSource = roles;
            cbRoleFilter.SelectedIndex = 0;
        }

        private void LoadUsers()
        {
            var userDAL = new UserDAL(new ViolationManagementContext());
            string keyword = NormalizeName(txtSearch.Text);
            string role = cbRoleFilter.SelectedItem?.ToString();
            // Nếu chọn "Tất cả" thì không lọc role
            if (string.IsNullOrEmpty(role) || role == "Tất cả")
            {
                role = null;
            }
            var users = userDAL.SearchUsers(keyword, role, currentPage, pageSize, out totalCount);
            dgUsers.ItemsSource = users;

            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            lblPageInfo.Text = $"Trang {currentPage}/{Math.Max(totalPages, 1)}";
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;
            LoadUsers();
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadUsers();
            }
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadUsers();
            }
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddAccount add = new AddAccount();
            add.ShowDialog();
            LoadRoles();
            LoadUsers();
        }
        public static string NormalizeName(string? input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            input = input.Trim();
            var words = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(" ", words);
        }

        private void EditAccount(object sender, RoutedEventArgs e)
        {
            if (dgUsers.SelectedItem != null)
            {
                var selectedUser = dgUsers.SelectedItem as User;
                EditAccount edit = new EditAccount(selectedUser.UserId);
                edit.ShowDialog();
                LoadUsers();
                LoadRoles();
            }
        }
    }
}
