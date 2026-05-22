using LiveCoding.Application.Generics;
using LiveCoding.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace LiveCoding.Application.UseCases.CreateOrder
{
    public class CreateOrderUseCase(IOrderService orderService, ICreateOrderValidation validation, ILogger<CreateOrderUseCase> logger) : ICreateOrderUseCase
    {
        public async Task<Response<CreateOrderOutput>> ExecuteAsync(CreateOrderInput input, CancellationToken cancellationToken)
        {
            logger.LogInformation("[CreateOrderUseCase][ExecuteAsync] Creating order with {ProductCount} products", input.Products?.Count);

            var validationResult = await validation.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                logger.LogWarning("[CreateOrderUseCase][ExecuteAsync] Validation failed: {Errors}", string.Join("; ", errors));
                return new Response<CreateOrderOutput>(false)
                    .AddErrors(errors);
            }

            try
            {
                var response = await orderService.CreateOrderAsync(input, cancellationToken);
                if (!response.IsSuccess)
                    return response;

                logger.LogInformation("[CreateOrderUseCase][ExecuteAsync] Order created successfully");
                return new Response<CreateOrderOutput>(true, response.Content);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[CreateOrderUseCase][ExecuteAsync] {Message}", ex.Message);
                return new Response<CreateOrderOutput>(false)
                    .AddError(ex.Message);
            }
        }
    }
}