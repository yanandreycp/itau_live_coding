using LiveCoding.Application.Generics;
using LiveCoding.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace LiveCoding.Application.UseCases.ChangeProductQuantity
{
    public class ChangeProductQuantityUseCase(IOrderService orderService, IChangeProductQuantityValidation validation, ILogger<ChangeProductQuantityUseCase> logger) : IChangeProductQuantityUseCase
    {
        public async Task<Response<ChangeProductQuantityOutput>> ExecuteAsync(ChangeProductQuantityInput input, CancellationToken cancellationToken)
        {
            logger.LogInformation("[ChangeProductQuantityUseCase][ExecuteAsync] Changing product {ProductId} quantity to {Quantity} in order {OrderId}",
                input.ProductId, input.Quantity, input.OrderId);

            var validationResult = await validation.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                logger.LogWarning("[ChangeProductQuantityUseCase][ExecuteAsync] Validation failed: {Errors}", string.Join("; ", errors));
                return new Response<ChangeProductQuantityOutput>(false)
                    .AddErrors(errors);
            }

            try
            {
                var response = await orderService.ChangeProductQuantityAsync(input, cancellationToken);
                if (!response.IsSuccess)
                    return response;

                logger.LogInformation("[ChangeProductQuantityUseCase][ExecuteAsync] Product quantity updated successfully");
                return new Response<ChangeProductQuantityOutput>(true, response.Content);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[ChangeProductQuantityUseCase][ExecuteAsync] {Message}", ex.Message);
                return new Response<ChangeProductQuantityOutput>(false)
                    .AddError(ex.Message);
            }
        }
    }
}