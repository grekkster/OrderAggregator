using Moq;
using OrderAggregator.Models;
using OrderAggregator.Services;

namespace OrderAggregator.UnitTests;

public class OrderServiceTest
{
    private readonly Mock<IProductStore> _productStoreMock = new Mock<IProductStore>();
    private readonly OrderService _sut;

    public OrderServiceTest()
    {
        _sut = new OrderService(_productStoreMock.Object);
    }

    [Theory]
    [MemberData(nameof(OrderData))]
    public void AddOrUpdateOrder_NonEmptyOrders_ShouldCallAddOrUpdateProduct(Order[] orders)
    {
        // Act
        _sut.AddOrUpdateOrder(orders);

        // Assert
        foreach (var order in orders)
        {
            _productStoreMock.Verify(p => p.AddOrUpdateProduct(order));
        }
    }

    [Fact]
    public void AddOrUpdateOrder_EmptyOrders_ShouldNotCallAddOrUpdateProduct()
    {
        // Act
        _sut.AddOrUpdateOrder(Array.Empty<Order>());
        
        // Assert
        _productStoreMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void GetAllOrders_ShouldCallGetAllOrders()
    {
        // Arrange
        // Act
        _sut.GetAllOrders();

        // Assert
        _productStoreMock.Verify(p => p.GetAllOrders());
    }

    public static IEnumerable<object[]> OrderData =>
    [
        // One
        [
            new Order[]
            {
                new() { ProductId = 1, Quantity = 1 }
            }
        ],
        // Multiple
        [
            new Order[]
            {
                new() { ProductId = 1, Quantity = 1 },
                new() { ProductId = 2, Quantity = 2 }
            }
        ],
        // Same
        [
            new Order[]
            {
                new() { ProductId = 1, Quantity = 1 },
                new() { ProductId = 1, Quantity = 1 },
            }
        ],
    ];
}
