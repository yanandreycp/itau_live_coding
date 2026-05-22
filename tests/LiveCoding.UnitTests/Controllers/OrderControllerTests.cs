using LiveCoding.Application.Generics;
using LiveCoding.Application.UseCases.ChangeProductQuantity;
using LiveCoding.Application.UseCases.CreateOrder;
using LiveCoding.Application.UseCases.GetOrder;
using LiveCoding.Application.UseCases.RemoveOrderProduct;
using LiveCoding.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LiveCoding.UnitTests.Controllers;

public class OrderControllerTests
{
    private readonly Mock<IGetOrderUseCase> _getOrderUseCaseMock = new();
    private readonly OrderController _controller;

    public OrderControllerTests()
    {
        var createMock = new Mock<ICreateOrderUseCase>();
        var changeMock = new Mock<IChangeProductQuantityUseCase>();
        var removeMock = new Mock<IRemoveOrderProductUseCase>();
        _controller = new OrderController(
            _getOrderUseCaseMock.Object,
            createMock.Object,
            changeMock.Object,
            removeMock.Object);
    }

    [Fact]
    public async Task GetOrder_WhenUseCaseReturnsSuccessWithContent_ReturnsOk()
    {
        _getOrderUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny<GetOrderInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Response<GetOrderOutput>(true, new GetOrderOutput { Id = Guid.NewGuid() }));

        var result = await _controller.GetOrder(Guid.NewGuid(), CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<GetOrderOutput>(okResult.Value);
    }

    [Fact]
    public async Task GetOrder_WhenUseCaseReturnsError_ReturnsBadRequest()
    {
        _getOrderUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny<GetOrderInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Response<GetOrderOutput>(false).AddError("error"));

        var result = await _controller.GetOrder(Guid.NewGuid(), CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetOrder_WhenUseCaseThrows_PropagatesException()
    {
        _getOrderUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny<GetOrderInput>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException());

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _controller.GetOrder(Guid.NewGuid(), CancellationToken.None));
    }
}
