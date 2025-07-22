using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using LibVLCSharp.Shared; // Thêm thư viện LibVLCSharp
using ViolationManagement.Controllers;

namespace ViolationManagement.Views
{
    public partial class ViewReport : Window
    {
        private readonly ReportController _controller = new();
        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;


        public ViewReport(int reportId)
        {
            InitializeComponent();

            Core.Initialize(); // Bắt buộc phải khởi tạo LibVLCSharp

            var report = _controller.GetReportById(reportId);

            if (report == null)
            {
                MessageBox.Show("Không tìm thấy phản ánh.");
                Close();
                return;
            }

            this.DataContext = report;

            LoadImage(report.ImageUrl);
            LoadVideo(report.VideoUrl);
        }

        private void LoadImage(string imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath)) return;

            try
            {
                string fullPath = ResolvePath(imagePath);

                if (File.Exists(fullPath))
                {
                    BitmapImage bitmap = new();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(fullPath, UriKind.Absolute);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    imgViewer.Source = bitmap;
                }
                else
                {
                    MessageBox.Show($"Ảnh không tồn tại:\n{fullPath}");
                }
            }   
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load ảnh:\n{ex.Message}");
            }
        }

        private void LoadVideo(string videoPath)
        {
            if (string.IsNullOrWhiteSpace(videoPath)) return;

            try
            {
                string fullPath = ResolvePath(videoPath);

                if (File.Exists(fullPath))
                {
                    _libVLC = new LibVLC();
                    _mediaPlayer = new MediaPlayer(_libVLC);
                    vlcPlayer.MediaPlayer = _mediaPlayer;

                    using var media = new Media(_libVLC, new Uri(fullPath));
                    _mediaPlayer.Play(media);
                }
                else
                {
                    MessageBox.Show($"Video không tồn tại:\n{fullPath}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load video:\n{ex.Message}");
            }
        }

        private string ResolvePath(string relativePath)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = Path.GetFullPath(Path.Combine(basePath, @"..\..\.."));
            return Path.Combine(projectRoot, relativePath.Replace('/', Path.DirectorySeparatorChar));
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            _mediaPlayer?.Play();
        }

        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            _mediaPlayer?.Pause();
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _mediaPlayer.Stop();
        }
    }
}