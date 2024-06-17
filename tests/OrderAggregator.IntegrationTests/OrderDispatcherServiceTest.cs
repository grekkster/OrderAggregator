using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderAggregator.Services;
using Moq;
using Microsoft.Extensions.Options;
using OrderAggregator.Configuration;

namespace OrderAggregator.IntegrationTests;

public class OrderDispatcherServiceTest
{
    private readonly Mock<IOrdersProcessor> _orderProcessorMock = new();
    private readonly Mock<IOrderService> _orderServiceMock = new();
    private readonly Mock<IOptionsMonitor<OrderDispatcherOptions>> _optionsMonitorMock = new();
    private readonly OrderDispatcherOptions _orderDispatcherOptions = new() { DispatcherTimerSeconds = 1 };

    [Fact]
    public async Task ExecuteAsync_CallsOrderService()
    {
        // Arrange
        IServiceCollection services = new ServiceCollection();
        services.AddScoped<IOrderService>(sp => _orderServiceMock.Object);
        services.AddSingleton<IOrdersProcessor>(sp => _orderProcessorMock.Object);
        _orderServiceMock.Setup(m => m.GetAllOrders()).Returns([]);
        _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(_orderDispatcherOptions);
        services.AddTransient<IOptionsMonitor<OrderDispatcherOptions>>(_ => _optionsMonitorMock.Object);

        services.AddHostedService<OrderDispatcherService>();
        var serviceProvider = services.BuildServiceProvider();

        var service = serviceProvider.GetService<IHostedService>() as OrderDispatcherService;

        var orderService = serviceProvider.GetService<IOrderService>();

        // Act
        await service!.StartAsync(CancellationToken.None);
        await Task.Delay(1500);

        // Assert
        _orderServiceMock?.Verify(m => m.GetAllOrders(), Times.Once());

        await service.StopAsync(CancellationToken.None);
    }
}
