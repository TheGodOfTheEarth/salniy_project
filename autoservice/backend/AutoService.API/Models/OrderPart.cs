using System;
using System.Collections.Generic;

namespace AutoService.API.Models;

public partial class OrderPart
{
    public int Id { get; set; }

    public int WorkOrderId { get; set; }

    public int PartId { get; set; }

    public int Quantity { get; set; }

    public decimal PriceAtMoment { get; set; }

    public virtual Part Part { get; set; } = null!;

    public virtual WorkOrder WorkOrder { get; set; } = null!;
}
