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

namespace ViolationManagement.Views
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Window
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void OpenHome(object sender, RoutedEventArgs e)
        {

        }
        private void OpenLookup(object sender, RoutedEventArgs e)
        {

        }
        private void OpenRegister(object sender, RoutedEventArgs e)
        {
            RegisterPage registerWindow = new RegisterPage();
            registerWindow.ShowDialog(); // Hoặc Show() nếu không muốn chặn
        }

        private void OpenLogin(object sender, RoutedEventArgs e)
        {
            Login loginWindow = new Login();
            loginWindow.ShowDialog(); // Hoặc Show()
        }
    }
}
