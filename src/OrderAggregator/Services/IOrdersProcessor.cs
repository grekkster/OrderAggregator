namespace OrderAggregator.Services;

public interface IOrdersProcessor
{
    void ProcessOrders(IDictionary<int, ulong> orders);
}
