using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderAggregator.Controllers;
using OrderAggregator.Models;
using OrderAggregator.Services;

namespace OrderAggregator.UnitTests;

public class OrderControllerTest
{
    private readonly Mock<IOrderService> _orderServiceMock = new();
    private readonly OrderController _sut;

    public OrderControllerTest()
    {
         _sut = new OrderController(_orderServiceMock.Object);
    }

    [Fact]
    public async Task PostOrders_NoOrder_ShouldReturnBadRequest()
    {
        // Act
        var result = await _sut.PostOrders(null!);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var resultValue = Assert.IsType<string>(badRequestResult.Value);
        Assert.Equal("No orders received.", resultValue);
    }

    [Fact]
    public async Task PostOrders_QuantityZero_ShouldReturnBadRequest()
    {
        // Ararnge
        var orders = new Order[] { new() { ProductId = 0, Quantity = 0 } };

        // Act
        var result = await _sut.PostOrders(orders);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var resultValue = Assert.IsType<string>(badRequestResult.Value);
        Assert.Equal("Quantity must be higher than 0.", resultValue);
    }

    [Fact]
    public async Task PostOrders_ShouldCallAddOrUpdateOrder()
    {
        // Ararnge
        var orders = new Order[] { new() { ProductId = 0, Quantity = 1 } };

        // Act
        var result = await _sut.PostOrders(orders);

        // Assert
        var noContentResult = Assert.IsType<NoContentResult>(result);
        _orderServiceMock.Verify(m => m.AddOrUpdateOrder(orders));
    }
}
