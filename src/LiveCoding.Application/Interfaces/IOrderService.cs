using LiveCoding.Application.Generics;
using LiveCoding.Application.UseCases.ChangeProductQuantity;
using LiveCoding.Application.UseCases.CreateOrder;
using LiveCoding.Application.UseCases.GetOrder;
using LiveCoding.Application.UseCases.RemoveOrderProduct;

namespace LiveCoding.Application.Interfaces
{
    public interface IOrderService
    {
        Task<Response<GetOrderOutput>> GetOrderAsync(Guid orderId, CancellationToken cancellationToken);
        Task<Response<CreateOrderOutput>> CreateOrderAsync(CreateOrderInput input, CancellationToken cancellationToken);
        Task<Response<ChangeProductQuantityOutput>> ChangeProductQuantityAsync(ChangeProductQuantityInput input, CancellationToken cancellationToken);
        Task<Response<RemoveOrderProductOutput>> RemoveOrderProductAsync(RemoveOrderProductInput input, CancellationToken cancellationToken);
    }
}