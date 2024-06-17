using OrderAggregator.Models;

namespace OrderAggregator.Services;

public interface IBatchStorage
{
    /// <summary>
    /// Adds orders to batch.
    /// </summary>
    /// <param name="orders">Orders to add.</param>
    /// <returns>Batched order count.</returns>
    public int AddToUpdateBatch(IEnumerable<Order> orders);

    /// <summary>
    /// Return all orders.
    /// </summary>
    /// <returns>All orders.</returns>
    public IEnumerable<Order> GetAll();

    /// <summary>
    /// Clear the batch.
    /// </summary>
    public void Clear();
}