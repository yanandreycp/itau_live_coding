using FluentValidation.Results;
using LiveCoding.Application.Generics;
using LiveCoding.Application.Interfaces;
using LiveCoding.Application.UseCases.CreateOrder;
using Moq;

namespace LiveCoding.UnitTests.UseCases;

public class CreateOrderUseCaseTests
{
    private readonly Mock<IOrderService> _serviceMock = new();
    private readonly CreateOrderUseCase _useCase;

    public CreateOrderUseCaseTests()
    {
        var validationMock = new Mock<ICreateOrderValidation>();
        validationMock
            .Setup(v => v.ValidateAsync(It.IsAny<CreateOrderInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _useCase = new CreateOrderUseCase(_serviceMock.Object, validationMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenValidationFails_ReturnsErrorResponse()
    {
        var validationMock = new Mock<ICreateOrderValidation>();
        validationMock
            .Setup(v => v.ValidateAsync(It.IsAny<CreateOrderInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("Products", "error") }));
        var useCase = new CreateOrderUseCase(_serviceMock.Object, validationMock.Object);

        var result = await useCase.ExecuteAsync(new CreateOrderInput(), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Contains("error", result.Errors);
    }

    [Fact]
    public async Task ExecuteAsync_WhenServiceReturnsSuccess_ReturnsSuccessResponse()
    {
        _serviceMock
            .Setup(s => s.CreateOrderAsync(It.IsAny<CreateOrderInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Response<CreateOrderOutput>(true, new CreateOrderOutput()));

        var result = await _useCase.ExecuteAsync(new CreateOrderInput(), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Content);
    }
}
