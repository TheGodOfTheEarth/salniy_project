using System;
using System.Collections.Generic;

namespace AutoService.API.Models;

public partial class WorkOrder
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public int CarId { get; set; }

    public int? ManagerId { get; set; }

    public int? MechanicId { get; set; }

    public int? BayId { get; set; }

    public string Status { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public string? Description { get; set; }

    public DateTime? ScheduledStart { get; set; }

    public DateTime? ScheduledEnd { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ClosedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual RepairBay? Bay { get; set; }

    public virtual Car Car { get; set; } = null!;

    public virtual User Client { get; set; } = null!;

    public virtual User? Manager { get; set; }

    public virtual User? Mechanic { get; set; }

    public virtual ICollection<OrderPart> OrderParts { get; set; } = new List<OrderPart>();

    public virtual ICollection<OrderService> OrderServices { get; set; } = new List<OrderService>();
}
