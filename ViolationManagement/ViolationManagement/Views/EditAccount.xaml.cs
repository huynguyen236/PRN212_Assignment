using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ViolationManagement.Controller;
using ViolationManagement.Controllers;
using ViolationManagement.DAL;
using ViolationManagement.Models;

namespace ViolationManagement.Views
{
    /// <summary>
    /// Interaction logic for EditAccount.xaml
    /// </summary>
    public partial class EditAccount : Window
    {
        private readonly UserDAL _userDAL = new UserDAL(new ViolationManagementContext());
        private readonly ViolationManagementContext _context = new ViolationManagementContext();
        private readonly EditAccountController _controller = new();
        private int _userId;
        public EditAccount(int userId)
        {
            InitializeComponent();
            _userId = userId;
            this.Loaded += EditAccount_Loaded;
        }
        private void EditAccount_Loaded(object sender, RoutedEventArgs e)
        {
            var user = _userDAL.GetUserById(_userId);
            if (user != null)
            {
                CitizenIdTextBox.Text = user.CitizenId;
                FullNameTextBox.Text = user.FullName;
                EmailTextBox.Text = user.Email;
                PhoneTextBox.Text = user.Phone;
                AddressTextBox.Text = user.Address;

                // Set selected item theo nội dung (Content)
                RoleComboBox.SelectedItem = RoleComboBox.Items
                    .OfType<ComboBoxItem>()
                    .FirstOrDefault(i => i.Content.ToString() == user.Role);

                GenderComboBox.SelectedItem = GenderComboBox.Items
                    .OfType<ComboBoxItem>()
                    .FirstOrDefault(i => i.Content.ToString() == user.Gender);

            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            string cccd = CitizenIdTextBox.Text.Trim();
            string email = EmailTextBox.Text.Trim();
            string phone = PhoneTextBox.Text.Trim();
            string fullName = FullNameTextBox.Text.Trim();
            string gender = (GenderComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string role = (RoleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string address = AddressTextBox.Text.Trim();

            bool success = _controller.EditAccount(cccd, email, phone, fullName, gender, address, role, out string message);

            if (success)
            {
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show(message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
