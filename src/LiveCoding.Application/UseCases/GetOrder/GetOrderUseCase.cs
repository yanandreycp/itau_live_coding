using LiveCoding.Application.Generics;
using LiveCoding.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace LiveCoding.Application.UseCases.GetOrder
{
    public class GetOrderUseCase(IOrderService orderService, IGetOrderValidation validation, ILogger<GetOrderUseCase> logger) : IGetOrderUseCase
    {
        public async Task<Response<GetOrderOutput>> ExecuteAsync(GetOrderInput input, CancellationToken cancellationToken)
        {
            logger.LogInformation("[GetOrderUseCase][ExecuteAsync] Getting order {OrderId}", input.Id);

            var validationResult = await validation.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                logger.LogWarning("[GetOrderUseCase][ExecuteAsync] Validation failed: {Errors}", string.Join("; ", errors));
                return new Response<GetOrderOutput>(false)
                    .AddErrors(errors);
            }

            try
            {
                var response = await orderService.GetOrderAsync(input.Id, cancellationToken);
                if (!response.IsSuccess)
                    return response;

                return new Response<GetOrderOutput>(true, response.Content);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[GetOrderUseCase][ExecuteAsync] {Message}", ex.Message);
                return new Response<GetOrderOutput>(false)
                    .AddError(ex.Message);
            }
        }
    }
}
