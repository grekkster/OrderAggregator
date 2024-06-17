using OrderAggregator.Models;

namespace OrderAggregator.Services;

public interface IOrderService
{
    void AddOrUpdateOrder(IEnumerable<Order> orders);
    void AddToUpdateBatch(IEnumerable<Order> orders);
    Dictionary<int, ulong> GetAllOrders();
}