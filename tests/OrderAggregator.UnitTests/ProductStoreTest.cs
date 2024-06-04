using OrderAggregator.Models;
using OrderAggregator.Services;

namespace OrderAggregator.UnitTests;

public class ProductStoreTest
{
    private readonly ProductStore _sut = new();

    [Fact]
    public void AddOrUpdateProduct_NonExisting_ShouldAddNew()
    {
        // Arrange
        var order = new Order() { ProductId = 1, Quantity = 2 };

        // Act
        _sut.AddOrUpdateProduct(order);

        // Assert
        Assert.True(_sut.GetAllOrders().Count == 1);
        Assert.True(_sut.GetAllOrders()[order.ProductId] == order.Quantity);
    }

    [Fact]
    public void AddOrUpdateProduct_AddExisting_ShouldAddUpdateExisting()
    {
        // Arrange
        var order = new Order() { ProductId = 1, Quantity = 2 };
        _sut.AddOrUpdateProduct(order);

        // Act
        order.Quantity = 1;
        _sut.AddOrUpdateProduct(order);

        // Assert
        Assert.True(_sut.GetAllOrders().Count == 1);
        Assert.True(_sut.GetAllOrders()[order.ProductId] == 3);
    }

    [Fact]
    public void AddOrUpdateProduct_AddMultiple_ShouldAddMultiple()
    {
        // Arrange
        var order1 = new Order() { ProductId = 1, Quantity = 1 };
        var order2 = new Order() { ProductId = 2, Quantity = 2 };

        // Act
        _sut.AddOrUpdateProduct(order1);
        _sut.AddOrUpdateProduct(order2);

        // Assert
        Assert.True(_sut.GetAllOrders().Count == 2);
        Assert.True(_sut.GetAllOrders()[order1.ProductId] == order1.Quantity);
        Assert.True(_sut.GetAllOrders()[order2.ProductId] == order2.Quantity);
    }

    [Fact]
    public void GetAllOrders_Empty_ShouldReturnEmpty()
    {
        // Arrange

        // Act

        // Assert
        Assert.True(_sut.GetAllOrders().Count == 0);
    }

    [Fact]
    public void GetAllOrders_Single_ShouldReturnSingle()
    {
        // Arrange
        var order = new Order() { ProductId = 1, Quantity = 1 };
        _sut.AddOrUpdateProduct(order);

        // Act
        // Assert
        Assert.True(_sut.GetAllOrders().Count == 1);
        Assert.True(_sut.GetAllOrders()[order.ProductId] == order.Quantity);
    }

    [Fact]
    public void GetAllOrders_Multiple_ShouldReturnMultiple()
    {
        // Arrange
        var order1 = new Order() { ProductId = 1, Quantity = 1 };
        var order2 = new Order() { ProductId = 2, Quantity = 2 };
        _sut.AddOrUpdateProduct(order1);
        _sut.AddOrUpdateProduct(order2);

        // Act
        // Assert
        Assert.True(_sut.GetAllOrders().Count == 2);
        Assert.True(_sut.GetAllOrders()[order1.ProductId] == order1.Quantity);
        Assert.True(_sut.GetAllOrders()[order2.ProductId] == order2.Quantity);
    }
}
