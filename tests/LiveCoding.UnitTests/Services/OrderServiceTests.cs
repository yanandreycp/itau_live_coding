using LiveCoding.Application.Services;
using LiveCoding.Application.UseCases.ChangeProductQuantity;
using LiveCoding.Application.UseCases.CreateOrder;
using LiveCoding.Application.UseCases.RemoveOrderProduct;
using LiveCoding.Domain.Entities;
using LiveCoding.Domain.Enums;
using LiveCoding.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace LiveCoding.UnitTests.Services;

public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _repositoryMock = new();
    private readonly OrderService _service;

    public OrderServiceTests()
    {
        _service = new OrderService(_repositoryMock.Object, Mock.Of<ILogger<OrderService>>());
    }

    [Fact]
    public async Task GetOrderAsync_WhenOrderFound_ReturnsMappedOutput()
    {
        var orderId = Guid.NewGuid();
        _repositoryMock
            .Setup(r => r.GetOrderAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Order
            {
                Id = orderId,
                Products = new List<Product> { new() { Id = Guid.NewGuid(), Name = "P1", Quantity = 2, OriginalUnitPrice = 10m } },
                Type = EOrderType.Standard,
                CreatedAt = DateTime.UtcNow
            });

        var result = await _service.GetOrderAsync(orderId, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Content);
        Assert.Single(result.Content.Products);
    }

    [Fact]
    public async Task CreateOrderAsync_WhenStandard_CalculatesCorrectly()
    {
        var input = new CreateOrderInput
        {
            Products = new List<CreateOrderProductInput> { new() { Name = "P1", Quantity = 2, Price = 100m } },
            Type = EOrderType.Standard
        };

        Order? saved = null;
        _repositoryMock.Setup(r => r.SaveOrderAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .Callback<Order, CancellationToken>((o, _) => saved = o)
            .Returns(Task.CompletedTask);
        _repositoryMock.Setup(r => r.GetOrderAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => saved);

        await _service.CreateOrderAsync(input, CancellationToken.None);

        Assert.NotNull(saved);
        Assert.Equal(200m, saved.InitialPrice);
        Assert.Equal(200m, saved.EffectivePrice);
        Assert.Equal(0m, saved.Products[0].Rate);
    }

    [Fact]
    public async Task CreateOrderAsync_WhenExpress_Applies15PercentSurcharge()
    {
        var input = new CreateOrderInput
        {
            Products = new List<CreateOrderProductInput> { new() { Name = "P1", Quantity = 2, Price = 100m } },
            Type = EOrderType.Express
        };

        Order? saved = null;
        _repositoryMock.Setup(r => r.SaveOrderAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .Callback<Order, CancellationToken>((o, _) => saved = o)
            .Returns(Task.CompletedTask);
        _repositoryMock.Setup(r => r.GetOrderAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => saved);

        await _service.CreateOrderAsync(input, CancellationToken.None);

        Assert.NotNull(saved);
        Assert.Equal(200m, saved.InitialPrice);
        Assert.Equal(230m, saved.EffectivePrice);
        Assert.Equal(0.15m, saved.Products[0].Rate);
    }

    [Fact]
    public async Task CreateOrderAsync_WhenSubscription_Applies10PercentDiscount()
    {
        var input = new CreateOrderInput
        {
            Products = new List<CreateOrderProductInput> { new() { Name = "P1", Quantity = 2, Price = 100m } },
            Type = EOrderType.Subscription
        };

        Order? saved = null;
        _repositoryMock.Setup(r => r.SaveOrderAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .Callback<Order, CancellationToken>((o, _) => saved = o)
            .Returns(Task.CompletedTask);
        _repositoryMock.Setup(r => r.GetOrderAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => saved);

        await _service.CreateOrderAsync(input, CancellationToken.None);

        Assert.NotNull(saved);
        Assert.Equal(200m, saved.InitialPrice);
        Assert.Equal(180m, saved.EffectivePrice);
        Assert.Equal(-0.10m, saved.Products[0].Rate);
    }

    [Fact]
    public async Task ChangeProductQuantity_WhenProductExists_UpdatesAndRecalculatesPrice()
    {
        var orderId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var order = new Order
        {
            Id = orderId,
            Products = new List<Product> { new() { Id = productId, Name = "P1", Quantity = 1, OriginalUnitPrice = 100m } },
            Type = EOrderType.Express,
            CreatedAt = DateTime.UtcNow
        };

        _repositoryMock.Setup(r => r.GetOrderAsync(orderId, It.IsAny<CancellationToken>())).ReturnsAsync(order);
        _repositoryMock.Setup(r => r.UpdateOrderAsync(order, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _repositoryMock.Setup(r => r.GetOrderAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(order);

        var result = await _service.ChangeProductQuantityAsync(
            new ChangeProductQuantityInput(orderId, productId) { Quantity = 3 }, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(3, order.Products[0].Quantity);
        Assert.Equal(345m, order.EffectivePrice);
    }

    [Fact]
    public async Task RemoveOrderProduct_WhenProductExists_RemovesAndRecalculatesPrice()
    {
        var orderId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var product = new Product { Id = productId, Name = "P1", Quantity = 2, OriginalUnitPrice = 100m };
        var order = new Order
        {
            Id = orderId,
            Products = new List<Product> { product },
            Type = EOrderType.Express,
            CreatedAt = DateTime.UtcNow
        };

        _repositoryMock.Setup(r => r.GetOrderAsync(orderId, It.IsAny<CancellationToken>())).ReturnsAsync(order);
        _repositoryMock.Setup(r => r.UpdateOrderAsync(order, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _repositoryMock.Setup(r => r.GetOrderAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(order);

        var result = await _service.RemoveOrderProductAsync(
            new RemoveOrderProductInput(orderId, productId), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Empty(order.Products);
    }

    [Fact]
    public async Task WhenOrderNotFound_ReturnsError()
    {
        _repositoryMock
            .Setup(r => r.GetOrderAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Order?)null);

        var result = await _service.ChangeProductQuantityAsync(
            new ChangeProductQuantityInput(Guid.NewGuid(), Guid.NewGuid()) { Quantity = 1 }, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Contains("Order not found", result.Errors);
    }

    [Fact]
    public async Task WhenRepositoryThrows_ReturnsError()
    {
        _repositoryMock
            .Setup(r => r.GetOrderAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("db error"));

        var result = await _service.GetOrderAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Contains("db error", result.Errors);
    }
}
