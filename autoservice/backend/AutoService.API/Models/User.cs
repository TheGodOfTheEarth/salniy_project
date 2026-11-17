using System;
using System.Collections.Generic;

namespace AutoService.API.Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Specialization { get; set; }

    public DateOnly? HireDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();

    public virtual ICollection<WorkOrder> WorkOrderClients { get; set; } = new List<WorkOrder>();

    public virtual ICollection<WorkOrder> WorkOrderManagers { get; set; } = new List<WorkOrder>();

    public virtual ICollection<WorkOrder> WorkOrderMechanics { get; set; } = new List<WorkOrder>();
}
