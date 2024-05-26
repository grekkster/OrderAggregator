using OrderAggregator.Models;

namespace OrderAggregator.Services;

public interface IProductStore
{
    void AddOrUpdateProduct(Order order);

    Dictionary<int, ulong> GetAllOrders();
}
