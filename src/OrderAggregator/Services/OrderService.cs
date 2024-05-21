using System.Collections.Concurrent;

using OrderAggregator.Models;

namespace OrderAggregator.Services;

public class OrderService : IOrderService
{
    // TODO IRepository
    // TODO OrdersService + nějaká abstrakce, prozatím uložit do paměti
    private static readonly ConcurrentDictionary<int, ulong> Orders = new();

    public void AddOrUpdateOrder(IEnumerable<Order> orders)
    {
        foreach (var order in orders)
        {
            Orders.AddOrUpdate(order.ProductId, order.Quantity, (key, oldValue) => oldValue + order.Quantity);
        }
    }

    public Dictionary<int, ulong> GetAllOrders()
    {
        return Orders.ToDictionary();
        //return Orders.Select(item => new Order { ProductId = item.Key, Quantity = item.Value });
    }
}
