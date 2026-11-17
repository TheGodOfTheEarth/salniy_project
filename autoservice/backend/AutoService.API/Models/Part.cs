using System;
using System.Collections.Generic;

namespace AutoService.API.Models;

public partial class Part
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Article { get; set; }

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<OrderPart> OrderParts { get; set; } = new List<OrderPart>();
}
