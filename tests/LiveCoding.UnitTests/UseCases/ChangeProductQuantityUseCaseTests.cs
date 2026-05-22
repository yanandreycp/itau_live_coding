using FluentValidation.Results;
using LiveCoding.Application.Generics;
using LiveCoding.Application.Interfaces;
using LiveCoding.Application.UseCases.ChangeProductQuantity;
using Moq;

namespace LiveCoding.UnitTests.UseCases;

public class ChangeProductQuantityUseCaseTests
{
    private readonly Mock<IOrderService> _serviceMock = new();
    private readonly ChangeProductQuantityUseCase _useCase;

    public ChangeProductQuantityUseCaseTests()
    {
        var validationMock = new Mock<IChangeProductQuantityValidation>();
        validationMock
            .Setup(v => v.ValidateAsync(It.IsAny<ChangeProductQuantityInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _useCase = new ChangeProductQuantityUseCase(_serviceMock.Object, validationMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenValidationFails_ReturnsErrorResponse()
    {
        var validationMock = new Mock<IChangeProductQuantityValidation>();
        validationMock
            .Setup(v => v.ValidateAsync(It.IsAny<ChangeProductQuantityInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("Quantity", "error") }));
        var useCase = new ChangeProductQuantityUseCase(_serviceMock.Object, validationMock.Object);

        var result = await useCase.ExecuteAsync(new ChangeProductQuantityInput(Guid.NewGuid(), Guid.NewGuid()), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Contains("error", result.Errors);
    }

    [Fact]
    public async Task ExecuteAsync_WhenServiceReturnsSuccess_ReturnsSuccessResponse()
    {
        _serviceMock
            .Setup(s => s.ChangeProductQuantityAsync(It.IsAny<ChangeProductQuantityInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Response<ChangeProductQuantityOutput>(true, new ChangeProductQuantityOutput()));

        var result = await _useCase.ExecuteAsync(new ChangeProductQuantityInput(Guid.NewGuid(), Guid.NewGuid()) { Quantity = 1 }, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Content);
    }
}
