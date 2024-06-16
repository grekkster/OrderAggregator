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

    [HttpPost]
    //public async Task<ActionResult> PostOrders(Order[] orders)
    //public async Task<ActionResult> PostOrdersAsync([FromBody] Order[] orders)
    public async Task<ActionResult> PostOrders([FromBody] Order[] orders)
    //public ActionResult PostOrders([FromBody] Order[] orders)
    {
        if (orders is null)
        {
            return BadRequest(Constants.BadRequestNoOrdersReceived);
        }

        if (orders.Any(o => o.Quantity <= 0))
        {
            return BadRequest(Constants.BadRequestQuantity);
        }

        _orderService.AddOrUpdateOrder(orders);

        return NoContent();
    }
}
