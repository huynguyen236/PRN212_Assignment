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

namespace ViolationManagement
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {

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

        private void ForgotPassword_Click(object sender, MouseButtonEventArgs e)
        {

        }

        private void OpenRegister_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
