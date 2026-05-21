using LiveCoding.Application.Repositories;
using LiveCoding.Application.UseCases.ChangeOrder;
using LiveCoding.Application.UseCases.CreateOrder;
using LiveCoding.Application.UseCases.GetOrder;
using LiveCoding.Infrastructure.Entities;
using LiveCoding.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LiveCoding.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _dbContext;

        public OrderRepository(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ChangeOrderOutput> ChangeOrderAsync(ChangeOrderInput input, CancellationToken cancellation)
        {
            var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == input.Id, cancellation);
            if (order == null)
                throw new Exception("Order not found");
            order.ProductId = input.ProductId;
            order.OrderType = input.Type;
            order.UpdatedAt = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync(cancellation);
            return new ChangeOrderOutput
            {
                Id = order.Id,
                ProductId = order.ProductId,
                Quantity = input.Quantity,
                Price = input.Price,
                Type = order.OrderType
            };
        }

        public async Task<CreateOrderOutput> CreateOrderAsync(CreateOrderInput input, CancellationToken cancellation)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                ProductId = input.ProductId,
                OrderType = input.Type,
                CreatedAt = DateTime.UtcNow
            };
            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync(cancellation);
            return new CreateOrderOutput
            {
                Id = order.Id,
                ProductId = order.ProductId,
                Quantity = input.Quantity,
                Price = input.Price,
                Type = order.OrderType
            };
        }

        public async Task<GetOrderOutput> GetOrderAsync(GetOrderInput input, CancellationToken cancellation)
        {
            var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == input.Id, cancellation);
            if (order == null)
                throw new Exception("Order not found");
            return new GetOrderOutput
            {
                Id = order.Id
            };
        }
    }
}