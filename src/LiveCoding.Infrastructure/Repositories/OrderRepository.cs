using LiveCoding.Domain.Entities;
using LiveCoding.Infrastructure.Interfaces;
using LiveCoding.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LiveCoding.Infrastructure.Repositories
{
    public class OrderRepository(OrderDbContext dbContext) : IOrderRepository
    {

        public async Task<Order?> GetOrderAsync(Guid orderId, CancellationToken cancellationToken)
        {
            return await dbContext.Orders
                .Include(o => o.Products)
                .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
        }

        public async Task SaveOrderAsync(Order order, CancellationToken cancellationToken)
        {
            dbContext.Orders.Add(order);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateOrderAsync(Order order, CancellationToken cancellationToken)
        {
            dbContext.Orders.Update(order);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}