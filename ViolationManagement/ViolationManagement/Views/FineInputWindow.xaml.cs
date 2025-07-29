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
    /// Interaction logic for FineInputWindow.xaml
    /// </summary>
    public partial class FineInputWindow : Window
    {
        public int? FineAmount { get; private set; }
        public FineInputWindow()
        {
            InitializeComponent();
        }
        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtFine.Text, out int value) || value < 0)
            {
                MessageBox.Show("Vui lòng nhập đúng định dạng tiền phạt", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show($"Xác nhận duyệt và phạt {value} VND?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                FineAmount = value;
                this.DialogResult = true;
                this.Close();
            }
        }
    }
}
