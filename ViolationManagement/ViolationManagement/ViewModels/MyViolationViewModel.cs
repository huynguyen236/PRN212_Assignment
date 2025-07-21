using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViolationManagement.ViewModels
{
    public class MyViolationViewModel
    {
        public string PlateNumber { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public DateTime FineDate { get; set; }
        public decimal FineAmount { get; set; }
        public bool PaidStatus { get; set; }  // bit trong DB ánh xạ thành bool

        public string PaidStatusText => PaidStatus ? "Đã thanh toán" : "Chưa thanh toán";
    }
}
