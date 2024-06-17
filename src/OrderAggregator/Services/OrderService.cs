using OrderAggregator.Models;

namespace OrderAggregator.Services;

public class OrderService(IProductStore productStore, IBatchStorage batchStorage) : IOrderService
{
    private readonly IProductStore _productStore = productStore;
    private readonly IBatchStorage _batchStorage = batchStorage;

    public void AddOrUpdateOrder(IEnumerable<Order> orders)
    {
        foreach (var order in orders)
        {
            _productStore.AddOrUpdateProduct(order);
        }
    }

    public void AddToUpdateBatch(IEnumerable<Order> orders)
    {
        var batchedOrderCount = _batchStorage.AddToUpdateBatch(orders);

        if (batchedOrderCount >= 5)
        {
            AddOrUpdateOrder(_batchStorage.GetAll());
            _batchStorage.Clear();
        }
    }

    public Dictionary<int, ulong> GetAllOrders() => _productStore.GetAllOrders();
}
