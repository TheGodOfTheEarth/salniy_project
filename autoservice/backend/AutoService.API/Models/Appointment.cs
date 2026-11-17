using System;
using System.Collections.Generic;

namespace AutoService.API.Models;

public partial class Appointment
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public int? CarId { get; set; }

    public int? ServiceId { get; set; }

    public DateOnly DesiredDate { get; set; }

    public TimeOnly DesiredTime { get; set; }

    public string? Comment { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Car? Car { get; set; }

    public virtual User Client { get; set; } = null!;

    public virtual Service? Service { get; set; }
}
