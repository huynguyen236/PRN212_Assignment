using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViolationManagement.ViewModels
{
    public class ReportDto
    {
        public int ReportId { get; set; }
        public string ReporterName { get; set; }
        public string ViolationType { get; set; }
        public string PlateNumber { get; set; }
        public DateTime ReportDate { get; set; }
        public string Status { get; set; }

    }
}
