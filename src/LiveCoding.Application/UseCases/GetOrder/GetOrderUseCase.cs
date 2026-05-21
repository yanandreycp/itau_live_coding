using LiveCoding.Application.Interfaces;

namespace LiveCoding.Application.UseCases.GetOrder
{
    public class GetOrderUseCase(IOrderService orderService) : IGetOrderUseCase
    {
        public async Task<GetOrderOutput> ExecuteAsync(GetOrderInput input, CancellationToken cancellationToken)
        {
            return await orderService.GetOrderAsync(input.Id, cancellationToken);
        }
    }
}
