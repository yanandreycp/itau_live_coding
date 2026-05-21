using LiveCoding.Application.Interfaces;

namespace LiveCoding.Application.UseCases.ChangeProductQuantity
{
    public class ChangeProductQuantityUseCase(IOrderService orderService) : IChangeProductQuantityUseCase
    {
        public async Task<ChangeProductQuantityOutput> ExecuteAsync(ChangeProductQuantityInput input, CancellationToken cancellationToken)
        {
            return await orderService.ChangeProductQuantityAsync(input, cancellationToken);
        }
    }
}