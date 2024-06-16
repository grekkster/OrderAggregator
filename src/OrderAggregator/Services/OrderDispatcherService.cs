
using Microsoft.Extensions.Options;
using OrderAggregator.Configuration;

namespace OrderAggregator.Services;

public class OrderDispatcherService(IServiceProvider services, IOrdersProcessor ordersProcessor) : BackgroundService
{
    private readonly IServiceProvider _services = services;
    private readonly IOrdersProcessor _ordersProcessor = ordersProcessor;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _services.CreateScope();
            var scopedOrderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
            var optionsDelegate = scope.ServiceProvider.GetRequiredService<IOptionsMonitor<OrderDispatcherOptions>>();

            // Get order dispatcher timer value from configuration and listen for changes.
            using PeriodicTimer timer = new(TimeSpan.FromSeconds(optionsDelegate.CurrentValue.DispatcherTimerSeconds));

            using var optionsChangeListener = optionsDelegate.OnChange((options) =>
            {
                timer.Period = TimeSpan.FromSeconds(options.DispatcherTimerSeconds);
                Console.WriteLine(Constants.DispatchTimerChanged, options.DispatcherTimerSeconds);
            });

            // Dispatch orders on timer.
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                Console.WriteLine(Constants.OrderDispatcherTimestamp, DateTime.Now, timer.Period.Seconds);

                _ordersProcessor.ProcessOrders(scopedOrderService.GetAllOrders());
            }
        }
    }
}
