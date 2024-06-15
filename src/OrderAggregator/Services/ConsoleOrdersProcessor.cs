
using System.Text.Json;

namespace OrderAggregator.Services;

public class ConsoleOrdersProcessor : IOrdersProcessor
{
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };

    public void ProcessOrders(IDictionary<int, ulong> orders)
    {
        var json = JsonSerializer.Serialize(orders, Options);
        Console.WriteLine(json);
    }
}
