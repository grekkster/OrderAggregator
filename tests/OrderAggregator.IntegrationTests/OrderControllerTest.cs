using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using OrderAggregator.Models;

namespace OrderAggregator.IntegrationTests;

public class OrderControllerTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public OrderControllerTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task PostOrders_NoOrder_ShouldReturnBadRequest()
    {
        // Arrange
        // Act
        var response = await _client.PostAsJsonAsync<Order[]>("/api/order", null!);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostOrders_QuantityZero_ShouldReturnBadRequest()
    {
        // Arrange
        var orders = new Order[] { new() { ProductId = 0, Quantity = 0 } };

        // Act
        var response = await _client.PostAsJsonAsync("/api/order", orders);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Equal("Quantity must be higher than 0.", responseString);
    }

    [Fact]
    public async Task PostOrders_ShouldCallAddOrUpdateOrder()
    {
        // Arrange
        var orders = new Order[] { new() { ProductId = 0, Quantity = 1 } };

        // Act
        var response = await _client.PostAsJsonAsync("/api/order", orders);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}