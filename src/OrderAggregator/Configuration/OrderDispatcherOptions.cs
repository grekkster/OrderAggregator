namespace OrderAggregator.Configuration;

public class OrderDispatcherOptions
{
    public const string Key = "OrderDispatcher";

    public int DispatcherTimerSeconds { get; set; } = 20;
}