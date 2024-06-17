using OrderAggregator.Models;

namespace OrderAggregator.Services;

public class BatchStorage : IBatchStorage
{
    private readonly List<Order> _batch = [];

    /// <inheritdoc />
    public int AddToUpdateBatch(IEnumerable<Order> orders)
    {
        _batch.AddRange(orders);
        return _batch.Count;
    }

    /// <inheritdoc />
    public void Clear() => _batch.Clear();

    /// <inheritdoc />
    public IEnumerable<Order> GetAll() => _batch;
}