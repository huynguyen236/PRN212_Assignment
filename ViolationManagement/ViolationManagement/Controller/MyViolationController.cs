using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ViolationManagement.Models;
using ViolationManagement.ViewModels;

namespace ViolationManagement.Controller
{
    internal class MyViolationController
    {
        private readonly ViolationManagementContext _context = new();



        public List<MyViolationViewModel> GetReportsByUserId(string userId)
        {
            var result = _context.Violations
                .Include(v => v.Report) 
                .Where(v => v.ViolatorId.ToString() == userId)
                .Select(v => new MyViolationViewModel
                {
                    PlateNumber = v.PlateNumber,
                    Location = v.Report.Location,
                    Description = v.Report.Description,
                    FineDate = v.FineDate ?? DateTime.MinValue,
                    FineAmount = v.FineAmount,
                    PaidStatus = v.PaidStatus
                }).ToList();

            return result;
        }

    }
}
