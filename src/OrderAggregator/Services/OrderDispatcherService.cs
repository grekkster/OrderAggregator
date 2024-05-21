
namespace OrderAggregator.Services;

public class OrderDispatcherService(IServiceProvider services) : BackgroundService
{
    private readonly IServiceProvider _service = services;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // TODO zastavit? stoppingToken
            using var scope = _service.CreateScope();
            var scopedOrderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
            //var orderService = scope.ServiceProvider.GetService<IOrderService>();

            // TODO timer z konfigurace
            using PeriodicTimer timer = new(TimeSpan.FromSeconds(3));

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                foreach (var order in scopedOrderService.GetAllOrders())
                //foreach (var order in orderService.GetAllOrders())
                {
                    Console.WriteLine(order);
                }
            }
        }
    }
}
