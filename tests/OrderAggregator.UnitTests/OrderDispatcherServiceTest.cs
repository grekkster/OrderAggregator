using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using OrderAggregator.Configuration;
using OrderAggregator.Services;

namespace OrderAggregator.UnitTests;

public class OrderDispatcherServiceTest
{
    private readonly Mock<IOrdersProcessor> _orderProcessorMock = new();
    private readonly Mock<IOrderService> _orderServiceMock = new();
    private readonly Mock<IOptionsMonitor<OrderDispatcherOptions>> _optionsMonitorMock = new();
    private readonly OrderDispatcherOptions _orderDispatcherOptions = new() { DispatcherTimerSeconds = 1 };

    [Fact]
    public async Task OrderDispatcherService_ExecuteAsync()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(_orderDispatcherOptions);
        _orderServiceMock.Setup(o => o.GetAllOrders()).Returns([]);

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<IOrderService>(_ => _orderServiceMock.Object);
        serviceCollection.AddTransient<IOptionsMonitor<OrderDispatcherOptions>>(_ => _optionsMonitorMock.Object);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var sut = new OrderDispatcherService(serviceProvider, _orderProcessorMock.Object);

        // Act
        await sut.StartAsync(CancellationToken.None);
        await Task.Delay(1500);
        await sut.StopAsync(CancellationToken.None);

        // Assert
        _orderServiceMock.Verify(o => o.GetAllOrders(), Times.Once);
    }
}