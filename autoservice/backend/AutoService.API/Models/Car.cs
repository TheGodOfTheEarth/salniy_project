using System;
using System.Collections.Generic;

namespace AutoService.API.Models;

public partial class Car
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Brand { get; set; } = null!;

    public string Model { get; set; } = null!;

    public int? Year { get; set; }

    public string LicensePlate { get; set; } = null!;

    public string? Vin { get; set; }

    public int? Mileage { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<WorkOrder> WorkOrders { get; set; } = new List<WorkOrder>();
}
