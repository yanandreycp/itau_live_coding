using FluentValidation.Results;
using LiveCoding.Application.Generics;
using LiveCoding.Application.Interfaces;
using LiveCoding.Application.UseCases.RemoveOrderProduct;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace LiveCoding.UnitTests.UseCases;

public class RemoveOrderProductUseCaseTests
{
    private readonly Mock<IOrderService> _serviceMock = new();
    private readonly RemoveOrderProductUseCase _useCase;

    public RemoveOrderProductUseCaseTests()
    {
        var validationMock = new Mock<IRemoveOrderProductValidation>();
        validationMock
            .Setup(v => v.ValidateAsync(It.IsAny<RemoveOrderProductInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _useCase = new RemoveOrderProductUseCase(_serviceMock.Object, validationMock.Object, Mock.Of<ILogger<RemoveOrderProductUseCase>>());
    }

    [Fact]
    public async Task ExecuteAsync_WhenValidationFails_ReturnsErrorResponse()
    {
        var validationMock = new Mock<IRemoveOrderProductValidation>();
        validationMock
            .Setup(v => v.ValidateAsync(It.IsAny<RemoveOrderProductInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("OrderId", "error") }));
        var useCase = new RemoveOrderProductUseCase(_serviceMock.Object, validationMock.Object, Mock.Of<ILogger<RemoveOrderProductUseCase>>());

        var result = await useCase.ExecuteAsync(new RemoveOrderProductInput(Guid.NewGuid(), Guid.NewGuid()), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Contains("error", result.Errors);
    }

    [Fact]
    public async Task ExecuteAsync_WhenServiceReturnsSuccess_ReturnsSuccessResponse()
    {
        _serviceMock
            .Setup(s => s.RemoveOrderProductAsync(It.IsAny<RemoveOrderProductInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Response<RemoveOrderProductOutput>(true, new RemoveOrderProductOutput()));

        var result = await _useCase.ExecuteAsync(new RemoveOrderProductInput(Guid.NewGuid(), Guid.NewGuid()), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Content);
    }
}
