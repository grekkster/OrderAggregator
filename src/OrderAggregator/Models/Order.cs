using System;

namespace OrderAggregator.Models;

/// <summary>
/// Order model.
/// </summary>
public class Order
{
    public int ProductId { get; set; }

    public ulong Quantity { get; set; }
}
