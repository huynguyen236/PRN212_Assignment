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
using ViolationManagement.Models;

namespace ViolationManagement.Views
{
    /// <summary>
    /// Interaction logic for ReportDetailWindow.xaml
    /// </summary>
    public partial class ReportDetailWindow : Window
    {
        private readonly ReportDAL _reportDAL = new ReportDAL(new ViolationManagementContext());
        public ReportDetailWindow(int reportId)
        {
            InitializeComponent();
            LoadReport(reportId);
        }
        private void LoadReport(int reportId)
        {
            Models.Report report = _reportDAL.GetReportById(reportId); // bạn cần tạo hàm này trong ReportDAL

            if (report == null)
            {
                MessageBox.Show("Không tìm thấy báo cáo.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }

            ViolationTypeText.Text = report.ViolationType;
            DescriptionText.Text = report.Description;
            PlateNumberText.Text = report.PlateNumber;
            LocationText.Text = report.Location;
            ReportDateText.Text = report.ReportDate.ToString();

            // Load ảnh
            if (!string.IsNullOrWhiteSpace(report.ImageUrl))
            {
                try
                {
                    ReportImage.Source = new BitmapImage(new Uri(report.ImageUrl));
                }
                catch
                {
                    MessageBox.Show("Không thể tải ảnh từ đường dẫn.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            // Load video
            if (!string.IsNullOrWhiteSpace(report.VideoUrl))
            {
                try
                {
                    ReportVideo.Source = new Uri(report.VideoUrl);
                    ReportVideo.LoadedBehavior = MediaState.Manual;
                    ReportVideo.UnloadedBehavior = MediaState.Manual;
                    ReportVideo.Play(); // hoặc giữ dừng, tùy thiết kế
                }
                catch
                {
                    MessageBox.Show("Không thể tải video từ đường dẫn.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }


        private void PlayVideo(object sender, RoutedEventArgs e)
        {
            ReportVideo.Play();
        }

        private void PauseVideo(object sender, RoutedEventArgs e)
        {
            ReportVideo.Pause();
        }
    }
}
