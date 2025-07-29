using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViolationManagement.Models;

namespace ViolationManagement.DAL
{
    public class ReportDAL
    {
        private readonly ViolationManagementContext _context;
        public ReportDAL(ViolationManagementContext context)
        {
            _context = context;
        }
        public Report GetReportById(int reportId)
        {
            return _context.Reports.FirstOrDefault(r => r.ReportId == reportId);
        }

        public List<Report> SearchReports(string keyword, int page, int pageSize, out int totalCount)
        {
            var query = _context.Reports.Include(r => r.Reporter)
                        .OrderByDescending(r => r.ReportDate)
                        .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(r => r.Reporter != null && r.Reporter.FullName.Contains(keyword));

            totalCount = query.Count();

            return query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }


        public void ApproveReport(int reportId)
        {
            var report = _context.Reports.FirstOrDefault(r => r.ReportId == reportId);
            if (report != null)
            {
                report.Status = "Approved";
                _context.Notifications.Add(new Notification
                {
                    UserId = report.ReporterId,
                    Message = "Báo cáo của bạn đã được duyệt.",
                    PlateNumber = report.PlateNumber
                });
                _context.SaveChanges();
            }
        }
        public void UpdateReport(Report report)
        {
            _context.Reports.Update(report);
            _context.SaveChanges();
        }

        public Vehicle GetVehicleByPlateNumber(string plateNumber)
        {
            return _context.Vehicles.FirstOrDefault(v => v.PlateNumber == plateNumber);
        }

        public void AddViolation(Violation violation)
        {
            _context.Violations.Add(violation);
            _context.SaveChanges();
        }

        public void AddNotification(Notification notification)
        {
            _context.Notifications.Add(notification);
            _context.SaveChanges();
        }


        public void RejectReport(int reportId)
        {
            var report = _context.Reports.FirstOrDefault(r => r.ReportId == reportId);
            if (report != null)
            {
                report.Status = "Rejected";
                _context.Notifications.Add(new Notification
                {
                    UserId = report.ReporterId,
                    Message = "Báo cáo của bạn đã bị từ chối.",
                    PlateNumber = report.PlateNumber
                });
                _context.SaveChanges();
            }
        }

    }
}
