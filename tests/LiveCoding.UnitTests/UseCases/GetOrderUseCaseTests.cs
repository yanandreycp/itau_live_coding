using FluentValidation.Results;
using LiveCoding.Application.Generics;
using LiveCoding.Application.Interfaces;
using LiveCoding.Application.UseCases.GetOrder;
using Moq;

namespace LiveCoding.UnitTests.UseCases;

public class GetOrderUseCaseTests
{
    private readonly Mock<IOrderService> _serviceMock = new();
    private readonly GetOrderUseCase _useCase;

    public GetOrderUseCaseTests()
    {
        var validationMock = new Mock<IGetOrderValidation>();
        validationMock
            .Setup(v => v.ValidateAsync(It.IsAny<GetOrderInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _useCase = new GetOrderUseCase(_serviceMock.Object, validationMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenValidationFails_ReturnsErrorResponse()
    {
        var validationMock = new Mock<IGetOrderValidation>();
        validationMock
            .Setup(v => v.ValidateAsync(It.IsAny<GetOrderInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("Id", "error") }));
        var useCase = new GetOrderUseCase(_serviceMock.Object, validationMock.Object);

        var result = await useCase.ExecuteAsync(new GetOrderInput(), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Contains("error", result.Errors);
    }

    [Fact]
    public async Task ExecuteAsync_WhenServiceReturnsSuccess_ReturnsSuccessResponse()
    {
        _serviceMock
            .Setup(s => s.GetOrderAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Response<GetOrderOutput>(true, new GetOrderOutput()));

        var result = await _useCase.ExecuteAsync(new GetOrderInput { Id = Guid.NewGuid() }, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Content);
    }
}
