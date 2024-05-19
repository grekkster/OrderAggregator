using OrderAggregator.Models;

namespace OrderAggregator.Services;

public class OrderService : IOrderService
{
    public void AddOrUpdateOrder(IEnumerable<Order> orders)
    {
        throw new NotImplementedException();
    }

    public Dictionary<int, ulong> GetAllOrders()
    {
        throw new NotImplementedException();
    }
}
