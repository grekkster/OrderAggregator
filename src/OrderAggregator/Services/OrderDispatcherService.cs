
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
            // TODO zastavit? stoppingToken, možná alespoň logging - zastaveno, dokončeno atp..
            // https://github.com/TechMinder/DynamicConfigurationChanges/blob/master/BackgroundService/ConfiugrationHostedService.cs
            using var scope = _services.CreateScope();
            var scopedOrderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
            
            var optionsDelegate = scope.ServiceProvider.GetRequiredService<IOptionsMonitor<OrderDispatcherOptions>>();

            using PeriodicTimer timer = new(TimeSpan.FromSeconds(optionsDelegate.CurrentValue.DispatcherTimerSeconds));

            using var optionsChangeListener = optionsDelegate.OnChange((options) =>
            {
                timer.Period = TimeSpan.FromSeconds(options.DispatcherTimerSeconds);
                Console.WriteLine("Dispatch timer changed: " + options.DispatcherTimerSeconds);
            });

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                // TODO nahradit rozhraním
                Console.WriteLine($"Now: {DateTime.Now}, Reload time: {timer.Period.Seconds}, Orders:");
                //foreach (var order in scopedOrderService.GetAllOrders())
                //{
                //    Console.WriteLine(order);
                //}
                _ordersProcessor.ProcessOrders(scopedOrderService.GetAllOrders());
            }
        }
    }
}
