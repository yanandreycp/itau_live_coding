using LiveCoding.Application.Generics;
using LiveCoding.Application.Interfaces;

namespace LiveCoding.Application.UseCases.CreateOrder
{
    public class CreateOrderUseCase(IOrderService orderService, ICreateOrderValidation validation) : ICreateOrderUseCase
    {
        public async Task<Response<CreateOrderOutput>> ExecuteAsync(CreateOrderInput input, CancellationToken cancellationToken)
        {
            var validationResult = await validation.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                return new Response<CreateOrderOutput>(false)
                    .AddError(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            try
            {
                var response = await orderService.CreateOrderAsync(input, cancellationToken);
                return new Response<CreateOrderOutput>(true, response.Content);
            }
            catch (Exception ex)
            {
                return new Response<CreateOrderOutput>(false)
                    .AddError(ex.Message);
            }
        }
    }
}