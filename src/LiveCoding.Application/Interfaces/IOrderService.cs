using LiveCoding.Application.UseCases.ChangeProductQuantity;
using LiveCoding.Application.UseCases.CreateOrder;
using LiveCoding.Application.UseCases.GetOrder;
using LiveCoding.Application.UseCases.RemoveOrderProduct;

namespace LiveCoding.Application.Interfaces
{
    public interface IOrderService
    {
        Task<GetOrderOutput> GetOrderAsync(Guid orderId, CancellationToken cancellationToken);
        Task<CreateOrderOutput> CreateOrderAsync(CreateOrderInput input, CancellationToken cancellationToken);
        Task<ChangeProductQuantityOutput> ChangeProductQuantityAsync(ChangeProductQuantityInput input, CancellationToken cancellationToken);
        Task<RemoveOrderProductOutput> RemoveOrderProductAsync(RemoveOrderProductInput input, CancellationToken cancellationToken);
    }
}