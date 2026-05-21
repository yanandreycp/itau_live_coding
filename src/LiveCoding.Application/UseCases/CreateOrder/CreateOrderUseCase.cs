using LiveCoding.Application.Interfaces;

namespace LiveCoding.Application.UseCases.CreateOrder
{
    public class CreateOrderUseCase(IOrderService orderService) : ICreateOrderUseCase
    {
        public async Task<CreateOrderOutput> ExecuteAsync(CreateOrderInput input, CancellationToken cancellationToken)
        {
            return await orderService.CreateOrderAsync(input, cancellationToken);
        }
    }
}