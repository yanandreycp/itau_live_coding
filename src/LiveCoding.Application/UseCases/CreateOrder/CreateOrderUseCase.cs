using LiveCoding.Application.Repositories;

namespace LiveCoding.Application.UseCases.CreateOrder
{
    public class CreateOrderUseCase(IOrderRepository orderRepository) : ICreateOrderUseCase
    {
        public async Task<CreateOrderOutput> CreateOrderAsync(CreateOrderInput input, CancellationToken cancellation)
        {
            return await orderRepository.CreateOrderAsync(input, cancellation);
        }
    }
}