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

    // TODO jen pro test, asi pak pryč
    [HttpGet]
    public ActionResult<Order[]> GetOrders() => Ok(_orderService.GetAllOrders());

    [HttpPost]
    //public async Task<ActionResult> PostOrders(Order[] orders)
    public async Task<ActionResult> PostOrders([FromBody] Order[] orders)
    {
        if (orders is null)
        {
            return BadRequest("No orders received."); // TODO konstanty
        }

        if (orders.Any(o => o.Quantity <= 0))
        {
            return BadRequest("Quantity must be higher than 0.");
        }

        _orderService.AddOrUpdateOrder(orders);

        return NoContent();
    }
}
