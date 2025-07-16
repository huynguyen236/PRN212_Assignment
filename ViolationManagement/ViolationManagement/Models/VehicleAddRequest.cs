using System;
using System.Collections.Generic;

namespace ViolationManagement.Models;

public partial class VehicleAddRequest
{
    public int RequestId { get; set; }

    public string PlateNumber { get; set; } = null!;

    public int OwnerId { get; set; }

    public string? Brand { get; set; }

    public string? Model { get; set; }

    public int? ManufactureYear { get; set; }

    public DateTime? RequestDate { get; set; }

    public string? Status { get; set; }

    public int? ApprovedBy { get; set; }

    public DateTime? ApprovalDate { get; set; }

    public virtual User? ApprovedByNavigation { get; set; }

    public virtual User Owner { get; set; } = null!;
}
