using LiveCoding.Application.UseCases.CreateOrder;
using LiveCoding.Domain.Entities;

namespace LiveCoding.Application.Extensions.Mapping
{
    public static class CreateOrderInputExtensions
    {
        public static Order ToOrder(this CreateOrderInput input)
        {
            return new Order
            {
                Id = Guid.NewGuid(),
                Products = input.Products.Select(p => new Product
                {
                    Id = Guid.NewGuid(),
                    Name = p.Name,
                    Quantity = p.Quantity,
                    OriginalUnitPrice = p.Price
                }).ToList(),
                Type = input.Type,
                InitialPrice = input.Products.Sum(p => p.Price * p.Quantity),
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}