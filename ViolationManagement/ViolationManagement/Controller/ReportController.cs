using Microsoft.Win32;
using System;
using System.IO;
using System.Security.Claims;
using System.Windows;
using ViolationManagement.Helper;
using ViolationManagement.Models;

namespace ViolationManagement.Controllers
{
    public class ReportController
    {
        private readonly ViolationManagementContext _context = new();

        public string? SelectFile(string title, string filter)
        {
            var dialog = new OpenFileDialog
            {
                Title = title,
                Filter = filter
            };

            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }

        public bool SubmitReport(string violationType, string description, string plateNumber, string location,
                          string imagePath, string videoPath, out string errorMessage)
        {
            errorMessage = "";

            // Lấy user ID từ claim
            string? userIdClaim = UserSession.GetClaim(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out int reporterId))
            {
                errorMessage = "Không xác định được người dùng. Vui lòng đăng nhập lại.";
                return false;
            }

            // Kiểm tra trường bắt buộc
            if (string.IsNullOrWhiteSpace(violationType))
            {
                errorMessage = "Loại vi phạm không được để trống.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(plateNumber))
            {
                errorMessage = "Biển số xe không được để trống.";
                return false;
            }
            var vehicleExists = _context.Vehicles.Any(v => v.PlateNumber == plateNumber);
            if (!vehicleExists)
            {
                errorMessage = "Không tìm thấy xe có biển số này trong hệ thống.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(location))
            {
                errorMessage = "Địa điểm không được để trống.";
                return false;
            }

            // Kiểm tra file ảnh
            if (string.IsNullOrWhiteSpace(imagePath) || !File.Exists(imagePath))
            {
                errorMessage = "Ảnh vi phạm không hợp lệ hoặc không tồn tại.";
                return false;
            }

            string imageExt = Path.GetExtension(imagePath).ToLower();
            var imageAllowed = new[] { ".png", ".jpg", ".jpeg", ".bmp" };
            if (!imageAllowed.Contains(imageExt))
            {
                errorMessage = "Ảnh phải có định dạng .png, .jpg, .jpeg hoặc .bmp.";
                return false;
            }

            long imageSize = new FileInfo(imagePath).Length;
            if (imageSize > 5 * 1024 * 1024)
            {
                errorMessage = "Ảnh vượt quá dung lượng cho phép (tối đa 5MB).";
                return false;
            }

            // Kiểm tra file video
            if (string.IsNullOrWhiteSpace(videoPath) || !File.Exists(videoPath))
            {
                errorMessage = "Video vi phạm không hợp lệ hoặc không tồn tại.";
                return false;
            }

            string videoExt = Path.GetExtension(videoPath).ToLower();
            var videoAllowed = new[] { ".mp4", ".avi", ".mov" };
            if (!videoAllowed.Contains(videoExt))
            {
                errorMessage = "Video phải có định dạng .mp4, .avi hoặc .mov.";
                return false;
            }

            long videoSize = new FileInfo(videoPath).Length;
            if (videoSize > 100 * 1024 * 1024)
            {
                errorMessage = "Video vượt quá dung lượng cho phép (tối đa 100MB).";
                return false;
            }
            try
            {
                // 5. Tạo thư mục lưu file
                string projectRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\"));
                string basePath = Path.Combine(projectRoot, "UploadedFiles");
                string imageFolder = Path.Combine(basePath, "Images");
                string videoFolder = Path.Combine(basePath, "Videos");
                Directory.CreateDirectory(imageFolder);
                Directory.CreateDirectory(videoFolder);

                // 6. Tạo tên file mới để tránh trùng
                string imageFileName = Guid.NewGuid().ToString() + imageExt;
                string videoFileName = Guid.NewGuid().ToString() + videoExt;

                string savedImagePath = Path.Combine(imageFolder, imageFileName);
                string savedVideoPath = Path.Combine(videoFolder, videoFileName);

                File.Copy(imagePath, savedImagePath, true);
                File.Copy(videoPath, savedVideoPath, true);

                // 7. Tạo bản ghi Report
                var report = new Report
                {
                    ViolationType = violationType,
                    Description = description,
                    PlateNumber = plateNumber,
                    Location = location,
                    ImageUrl = $"UploadedFiles/Images/{imageFileName}",
                    VideoUrl = $"UploadedFiles/Videos/{videoFileName}",
                    ReporterId = reporterId,
                    ReportDate = DateTime.Now,
                    Status = "Pending"
                };

                _context.Reports.Add(report);
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                string detailedMessage = ex.InnerException?.Message ?? ex.Message;
                errorMessage = "Lỗi hệ thống: " + detailedMessage;
                return false;
            }
        }
        public List<Report> GetReportsByUserId(string userId)
        {
            return _context.Reports
                .Where(r => r.ReporterId.ToString() == userId)
                .ToList();
        }

        public List<Report> GetReportsByPlate(string plate)
        {
            string keyword = plate.ToLower().Trim();
            return _context.Reports
                .Where(r => r.PlateNumber.Contains(keyword))
                .ToList();
        }
        public List<Report> GetReportsByStatus(string status, string userId)
        {
            if (string.IsNullOrEmpty(status) || status == "All")
            {
                return _context.Reports
                    .Where(r => r.ReporterId.ToString() == userId)
                    .ToList();
            }

            return _context.Reports
                .Where(r => r.ReporterId.ToString() == userId && r.Status == status)
                .ToList();
        }
        public Report? GetReportById(int reportId)
        {
            return _context.Reports.FirstOrDefault(r => r.ReportId == reportId);
        }
    }
}
