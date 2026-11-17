using System;
using System.Collections.Generic;

namespace AutoService.API.Models;

public partial class RepairBay
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<WorkOrder> WorkOrders { get; set; } = new List<WorkOrder>();
}
