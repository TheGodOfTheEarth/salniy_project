using System;
using System.Collections.Generic;

namespace AutoService.API.Models;

public partial class OrderService
{
    public int Id { get; set; }

    public int WorkOrderId { get; set; }

    public int ServiceId { get; set; }

    public int Quantity { get; set; }

    public decimal PriceAtMoment { get; set; }

    public virtual Service Service { get; set; } = null!;

    public virtual WorkOrder WorkOrder { get; set; } = null!;
}
