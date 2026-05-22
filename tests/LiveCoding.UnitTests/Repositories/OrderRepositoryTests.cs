using LiveCoding.Domain.Entities;
using LiveCoding.Domain.Enums;
using LiveCoding.Infrastructure.Persistence;
using LiveCoding.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace LiveCoding.UnitTests.Repositories;

public class OrderRepositoryTests : IDisposable
{
    private readonly DbConnection _connection;
    private readonly DbContextOptions<OrderDbContext> _options;

    public OrderRepositoryTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        _options = new DbContextOptionsBuilder<OrderDbContext>().UseSqlite(_connection).Options;
        using var ctx = new OrderDbContext(_options);
        ctx.Database.EnsureCreated();
    }

    public void Dispose() => _connection.Dispose();

    [Fact]
    public async Task GetOrderAsync_WhenExists_ReturnsOrderWithProducts()
    {
        var orderId = Guid.NewGuid();
        using (var ctx = new OrderDbContext(_options))
        {
            ctx.Orders.Add(new Order
            {
                Id = orderId,
                Products = new List<Product> { new() { Id = Guid.NewGuid(), Name = "P1", Quantity = 2, OriginalUnitPrice = 10m } },
                Type = EOrderType.Standard,
                CreatedAt = DateTime.UtcNow
            });
            ctx.SaveChanges();
        }

        using (var ctx = new OrderDbContext(_options))
        {
            var order = await new OrderRepository(ctx).GetOrderAsync(orderId, CancellationToken.None);

            Assert.NotNull(order);
            Assert.Single(order.Products!);
        }
    }

    [Fact]
    public async Task SaveOrderAsync_PersistsAndCanBeRetrieved()
    {
        var orderId = Guid.NewGuid();
        using (var ctx = new OrderDbContext(_options))
        {
            await new OrderRepository(ctx).SaveOrderAsync(new Order
            {
                Id = orderId,
                Products = new List<Product>(),
                Type = EOrderType.Express,
                InitialPrice = 200m,
                EffectivePrice = 230m,
                CreatedAt = DateTime.UtcNow
            }, CancellationToken.None);
        }

        using (var ctx = new OrderDbContext(_options))
        {
            var saved = await ctx.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
            Assert.NotNull(saved);
            Assert.Equal(230m, saved!.EffectivePrice);
        }
    }
}
