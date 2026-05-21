using LiveCoding.Domain.Entities;

namespace LiveCoding.Infrastructure.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetOrderAsync(Guid orderId, CancellationToken cancellationToken);
        Task SaveOrderAsync(Order order, CancellationToken cancellationToken);
        Task UpdateOrderAsync(Order order, CancellationToken cancellationToken);
    }
}