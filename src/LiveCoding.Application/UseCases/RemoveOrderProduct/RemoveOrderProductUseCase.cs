using LiveCoding.Application.Interfaces;

namespace LiveCoding.Application.UseCases.RemoveOrderProduct
{
    public class RemoveOrderProductUseCase(IOrderService orderService) : IRemoveOrderProductUseCase
    {
        public async Task<RemoveOrderProductOutput> ExecuteAsync(RemoveOrderProductInput input, CancellationToken cancellationToken)
        {
            return await orderService.RemoveOrderProductAsync(input, cancellationToken);
        }
    }
}