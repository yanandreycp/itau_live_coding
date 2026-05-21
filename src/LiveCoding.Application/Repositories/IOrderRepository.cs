using LiveCoding.Application.UseCases.ChangeOrder;
using LiveCoding.Application.UseCases.CreateOrder;
using LiveCoding.Application.UseCases.GetOrder;

namespace LiveCoding.Application.Repositories
{
    public interface IOrderRepository
    {
        Task<GetOrderOutput> GetOrderAsync(GetOrderInput input, CancellationToken cancellation);
        Task<CreateOrderOutput> CreateOrderAsync(CreateOrderInput input, CancellationToken cancellation);
        Task<ChangeOrderOutput> ChangeOrderAsync(ChangeOrderInput input, CancellationToken cancellation);
    }
}