using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ViolationManagement.Models;

namespace ViolationManagement.Controllers
{
    public partial class VehicleController
    {
    private readonly ViolationManagementContext _context;

    public VehicleController()
    {
        _context = new ViolationManagementContext();
    }
        private bool IsValidPlateNumber(string plateNumber)
        {
            if (string.IsNullOrWhiteSpace(plateNumber))
                return false;

            plateNumber = plateNumber.Trim().ToUpper();

            if (plateNumber.Contains('.'))
                return false;

            string pattern = @"^(?:[0-9]{2}[A-Z]{1}|[0-9]{2}[A-Z]{2})-[0-9]{5}$";
            return Regex.IsMatch(plateNumber, pattern);
        }

        public bool SubmitVehicleRequest(string plate, string brand, string model, int year, int ownerId, out string message)
        {
            message = "";
            try
            {
                var exists = _context.Vehicles.Any(v => v.PlateNumber == plate);
                if (exists)
                {
                    message = "Biển số đã tồn tại.";
                    return false;
                }
                if (IsValidPlateNumber(plate) == false)
                {
                    message = "Biển số không hợp lệ, ví dụ(30A-55555).";
                    return false;
                }

                var request = new VehicleAddRequest
                {
                    PlateNumber = plate,
                    OwnerId = ownerId,
                    Brand = brand,
                    Model = model,
                    ManufactureYear = year,
                    Status = "Pending",
                    RequestDate = DateTime.Now
                };

                _context.VehicleAddRequests.Add(request);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                message = "Lỗi khi gửi yêu cầu: " + ex.InnerException?.Message ?? ex.Message;
                return false;
            }
        }


    }
}

