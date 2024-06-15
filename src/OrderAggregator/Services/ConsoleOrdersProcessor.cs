
namespace OrderAggregator.Services;

public class ConsoleOrdersProcessor : IOrdersProcessor
{
    public void ProcessOrders(IDictionary<int, ulong> orders)
    {
        foreach (var order in orders)
        {
            Console.WriteLine(order.ToString());
        }
    }
}
