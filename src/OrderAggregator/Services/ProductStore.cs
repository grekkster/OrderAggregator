using System.Collections.Concurrent;
using OrderAggregator.Models;

namespace OrderAggregator.Services;

public class ProductStore : IProductStore
{
    private readonly ConcurrentDictionary<int, ulong> _orders = new();

    public void AddOrUpdateProduct(Order order) => 
        _orders.AddOrUpdate(order.ProductId, order.Quantity, (key, oldValue) => oldValue + order.Quantity);

    public Dictionary<int, ulong> GetAllOrders() => _orders.ToDictionary();
}
