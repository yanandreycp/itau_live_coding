using LiveCoding.Application.Generics;
using LiveCoding.Application.Interfaces;

namespace LiveCoding.Application.UseCases.GetOrder
{
    public class GetOrderUseCase(IOrderService orderService) : IGetOrderUseCase
    {
        public async Task<Response<GetOrderOutput>> ExecuteAsync(GetOrderInput input, CancellationToken cancellationToken)
        {
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
