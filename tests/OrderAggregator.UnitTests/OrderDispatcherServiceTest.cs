using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using OrderAggregator.Configuration;
using OrderAggregator.Models;
using OrderAggregator.Services;

namespace OrderAggregator.UnitTests;

public class OrderDispatcherServiceTest
{
    //private readonly Mock<IServiceProvider> _serviceProvideMock = new();
    private readonly Mock<IOrderService> _orderServiceMock = new();
    private readonly Mock<IOptionsMonitor<OrderDispatcherOptions>> _optionsMonitorMock = new();
    private readonly OrderDispatcherOptions _orderDispatcherOptions = new() { DispatcherTimerSeconds = 1 };
    private readonly ServiceProvider _serviceProvider;
    //private readonly OrderDispatcherService _sut;
    private OrderDispatcherService _sut;
    private readonly ServiceCollection _serviceCollection = new();

    public OrderDispatcherServiceTest()
    {
        //var serviceCollection = new ServiceCollection();
        //_serviceCollection.AddTransient<IOrderService>(_ => _orderServiceMock.Object);

        //_serviceProvider = serviceCollection.BuildServiceProvider();
        //_sut = new OrderDispatcherService(_serviceProvider);
    }

    [Fact]
    public async Task OrderDispatcherService_ExecuteAsync()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(_orderDispatcherOptions);
        //_orderServiceMock.Setup(o => o.GetAllOrders()).Returns([]).Callback(() => cts.CancelAfter(1500));
        //_orderServiceMock.Setup(o => o.GetAllOrders()).Returns([]).Callback(() => cts.Cancel());
        _orderServiceMock.Setup(o => o.GetAllOrders()).Returns([]);

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<IOrderService>(_ => _orderServiceMock.Object);
        serviceCollection.AddTransient<IOptionsMonitor<OrderDispatcherOptions>>(_ => _optionsMonitorMock.Object);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        _sut = new OrderDispatcherService(serviceProvider);

        // Act

        await _sut.StartAsync(CancellationToken.None);
        //await _sut.StartAsync(cts.Token); // !!!
        //cts.CancelAfter(1500);
        await Task.Delay(1500); // Nutný !!!
        //await _sut.ExecuteTask!.WaitAsync(cts.Token);
        await _sut.StopAsync(CancellationToken.None);
        //cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
        //await _sut.StopAsync(cts.Token);

        // Assert
        //Assert.True(_sut.ExecuteTask.IsCompletedSuccessfully);
        //_optionsMonitorMock.VerifyGet(o => o.CurrentValue.DispatcherTimerSeconds);
        _orderServiceMock.Verify(o => o.GetAllOrders(), Times.Once);
    }
}