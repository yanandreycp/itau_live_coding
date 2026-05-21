using LiveCoding.Application.Generics;
using LiveCoding.Application.Interfaces;

namespace LiveCoding.Application.UseCases.CreateOrder
{
    public class CreateOrderUseCase(IOrderService orderService) : ICreateOrderUseCase
    {
        public async Task<Response<CreateOrderOutput>> ExecuteAsync(CreateOrderInput input, CancellationToken cancellationToken)
        {
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