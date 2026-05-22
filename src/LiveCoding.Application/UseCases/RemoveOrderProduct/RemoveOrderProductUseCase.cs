using LiveCoding.Application.Generics;
using LiveCoding.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace LiveCoding.Application.UseCases.RemoveOrderProduct
{
    public class RemoveOrderProductUseCase(IOrderService orderService, IRemoveOrderProductValidation validation, ILogger<RemoveOrderProductUseCase> logger) : IRemoveOrderProductUseCase
    {
        public async Task<Response<RemoveOrderProductOutput>> ExecuteAsync(RemoveOrderProductInput input, CancellationToken cancellationToken)
        {
            logger.LogInformation("[RemoveOrderProductUseCase][ExecuteAsync] Removing product {ProductId} from order {OrderId}",
                input.ProductId, input.OrderId);

            var validationResult = await validation.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                logger.LogWarning("[RemoveOrderProductUseCase][ExecuteAsync] Validation failed: {Errors}", string.Join("; ", errors));
                return new Response<RemoveOrderProductOutput>(false)
                    .AddErrors(errors);
            }

            try
            {
                var response = await orderService.RemoveOrderProductAsync(input, cancellationToken);
                if (!response.IsSuccess)
                    return response;

                logger.LogInformation("[RemoveOrderProductUseCase][ExecuteAsync] Product removed successfully");
                return new Response<RemoveOrderProductOutput>(true, response.Content);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[RemoveOrderProductUseCase][ExecuteAsync] {Message}", ex.Message);
                return new Response<RemoveOrderProductOutput>(false)
                    .AddError(ex.Message);
            }
        }
    }
}