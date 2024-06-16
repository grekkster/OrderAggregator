
using System.Text.Json;

using OrderAggregator.Models;

namespace OrderAggregator.Services;

public class ConsoleOrdersProcessor : IOrdersProcessor
{
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };

    public void ProcessOrders(IDictionary<int, ulong> orderDictionary)
    {
        var orders = new List<Order>();
        foreach (var order in orderDictionary)
        {
            orders.Add(new Order() { ProductId = order.Key, Quantity = order.Value });
        }
        var json = JsonSerializer.Serialize(orders, Options);
        Console.WriteLine(json);
    }
}
