using OrderAggregator.Models;

namespace OrderAggregator.Services;

public class OrderService(IProductStore productStore) : IOrderService
{
    private readonly IProductStore _productStore = productStore;

    public void AddOrUpdateOrder(IEnumerable<Order> orders)
    {
        foreach (var order in orders)
        {
            _productStore.AddOrUpdateProduct(order);
        }
    }

    public Dictionary<int, ulong> GetAllOrders() => _productStore.GetAllOrders();
}
