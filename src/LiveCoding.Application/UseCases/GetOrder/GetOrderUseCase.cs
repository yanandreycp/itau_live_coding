using LiveCoding.Application.Generics;
using LiveCoding.Application.Interfaces;

namespace LiveCoding.Application.UseCases.GetOrder
{
    public class GetOrderUseCase(IOrderService orderService, IGetOrderValidation validation) : IGetOrderUseCase
    {
        public async Task<Response<GetOrderOutput>> ExecuteAsync(GetOrderInput input, CancellationToken cancellationToken)
        {
            var validationResult = await validation.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                return new Response<GetOrderOutput>(false)
                    .AddError(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            try
            {
                var response = await orderService.GetOrderAsync(input.Id, cancellationToken);
                return new Response<GetOrderOutput>(true, response.Content);
            }
            catch (Exception ex)
            {
                return new Response<GetOrderOutput>(false)
                    .AddError(ex.Message);
            }
        }
    }
}
