using Microsoft.AspNetCore.Mvc;
using OrderAggregator.Models;
using OrderAggregator.Services;

namespace OrderAggregator.Controllers;

/// <summary>
/// Orders processing controller.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class OrderController(IOrderService orderService) : ControllerBase
{
    private readonly IOrderService _orderService = orderService;

    // Potentional upgrade - make async, if _orderService async api is available.
    [HttpPost]
    public ActionResult PostOrders([FromBody] Order[] orders)
    {
        if (orders is null)
        {
            return BadRequest(Constants.BadRequestNoOrdersReceived);
        }

        if (orders.Any(o => o.Quantity <= 0))
        {
            return BadRequest(Constants.BadRequestQuantity);
        }

        // Upgrade - instead of directly updating orders for every request, collect them until treshold count is reached, then update multiple oders in one update call.
        // - instead of using _orderService directly, use:
        //_orderService.AddToUpdateBatch(orders);

        _orderService.AddOrUpdateOrder(orders);

        return NoContent();
    }
}
