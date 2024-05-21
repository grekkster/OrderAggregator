using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using OrderAggregator.Models;
using OrderAggregator.Services;

namespace OrderAggregator.Controllers;

/// <summary>
/// Summary description for Class1
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class OrderController(IOrderService orderService) : ControllerBase
{
    private readonly IOrderService _orderService = orderService;

    [HttpGet]
    public ActionResult<Order[]> GetOrders() =>
        //Ok(Orders.Select(item => new Order { ProductId = item.Key, Quantity = item.Value }));
        Ok(_orderService.GetAllOrders());

    [HttpPost]
    public async Task<ActionResult> PostOrders(Order[] orders)
    {
        //foreach (var order in orders)
        //{
        //    Orders.AddOrUpdate(order.ProductId, order.Quantity, (key, oldValue) => oldValue + order.Quantity);
        //}
        _orderService.AddOrUpdateOrder(orders);

        return Created();
    }
}
