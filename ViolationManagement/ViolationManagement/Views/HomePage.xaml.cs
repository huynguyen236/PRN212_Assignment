using System.Windows;

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
            registerWindow.Show();
            this.Close();// Hoặc Show() nếu không muốn chặn
        }

        private void OpenLogin(object sender, RoutedEventArgs e)
        {
            Login loginWindow = new Login();
            loginWindow.Show();
            this.Close();
        }
    }
}
