using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using OrderAggregator.Services;
using Moq;
using Microsoft.Extensions.Options;
using OrderAggregator.Configuration;

namespace OrderAggregator.IntegrationTests;

public class OrderDispatcherServiceTest
{
    private readonly OrderDispatcherService _orderDispatcherService;
    private readonly Mock<IOrderService> _orderServiceMock = new Mock<IOrderService>();
    private readonly Mock<IOptionsMonitor<OrderDispatcherOptions>> _optionsMonitorMock = new();
    private readonly OrderDispatcherOptions _orderDispatcherOptions = new() { DispatcherTimerSeconds = 1 };

    [Fact]
    public async Task ExecuteAsync_CallsOrderService()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddScoped<IOrderService>(sp => _orderServiceMock.Object);
        _orderServiceMock.Setup(m => m.GetAllOrders()).Returns(new Dictionary<int, ulong>());
        _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(_orderDispatcherOptions);
        services.AddTransient<IOptionsMonitor<OrderDispatcherOptions>>(_ => _optionsMonitorMock.Object);

        services.AddHostedService<OrderDispatcherService>();
        var serviceProvider = services.BuildServiceProvider();

        var service = serviceProvider.GetService<IHostedService>() as OrderDispatcherService;

        var orderService = serviceProvider.GetService<IOrderService>();

        await service!.StartAsync(CancellationToken.None);

        await Task.Delay(1000);
        _orderServiceMock?.Verify(m => m.GetAllOrders(), Times.Once());

        await service.StopAsync(CancellationToken.None);
    }
}
