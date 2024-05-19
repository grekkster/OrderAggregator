using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using OrderAggregator.Models;

namespace OrderAggregator.Controllers;

/// <summary>
/// Summary description for Class1
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    // TODO OrdersService + nějaká abstrakce, prozatím uložit do paměti
    private static readonly ConcurrentDictionary<int, ulong> Orders = new();

    [HttpGet]
    public ActionResult<Order[]> GetOrders() =>
        Ok(Orders.Select(item => new Order { ProductId = item.Key, Quantity = item.Value }));

    [HttpPost]
    public async Task<ActionResult> PostOrders(Order[] orders)
    {
        foreach (var order in orders)
        {
            Orders.AddOrUpdate(order.ProductId, order.Quantity, (key, oldValue) => oldValue + order.Quantity);
        }

        return Created();
    }
}
