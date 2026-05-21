using LiveCoding.Application.UseCases.ChangeProductQuantity;
using LiveCoding.Application.UseCases.CreateOrder;
using LiveCoding.Application.UseCases.GetOrder;
using LiveCoding.Application.UseCases.RemoveOrderProduct;
using LiveCoding.Domain.Entities;

namespace LiveCoding.Application.Extensions.Mapping
{
    public static class OrderExtensions
    {
        public static GetOrderOutput ToGetOrderOutput(this Order order)
        {
            return new GetOrderOutput
            {
                Id = order.Id,
                Products = order.Products.Select(p => new GetOrderProductOutput
                {
                    Id = p.Id,
                    Name = p.Name,
                    Quantity = p.Quantity,
                    OriginalUnitPrice = p.OriginalUnitPrice,
                    EffectiveUnitPrice = p.EffectiveUnitPrice,
                    Rate = p.Rate,
                    Delta = p.Delta,
                    TotalPrice = p.TotalPrice
                }).ToList(),
                Type = order.Type,
                InitialPrice = order.InitialPrice,
                EffectivePrice = order.EffectivePrice,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt
            };
        }

        public static CreateOrderOutput ToCreateOrderOutput(this Order order)
        {
            return new CreateOrderOutput
            {
                Id = order.Id,
                Products = order.Products.Select(p => new CreateOrderProductOutput
                {
                    Id = p.Id,
                    Name = p.Name,
                    Quantity = p.Quantity,
                    OriginalUnitPrice = p.OriginalUnitPrice,
                    EffectiveUnitPrice = p.EffectiveUnitPrice,
                    Rate = p.Rate,
                    Delta = p.Delta,
                    TotalPrice = p.TotalPrice
                }).ToList(),
                Type = order.Type,
                InitialPrice = order.InitialPrice,
                EffectivePrice = order.EffectivePrice,
                CreatedAt = order.CreatedAt
            };
        }

        public static ChangeProductQuantityOutput ToChangeProductQuantityOutput(this Order order)
        {
            return new ChangeProductQuantityOutput
            {
                Id = order.Id,
                Products = order.Products.Select(p => new ChangeProductQuantityProductOutput
                {
                    Id = p.Id,
                    Name = p.Name,
                    Quantity = p.Quantity,
                    OriginalUnitPrice = p.OriginalUnitPrice,
                    EffectiveUnitPrice = p.EffectiveUnitPrice,
                    Rate = p.Rate,
                    Delta = p.Delta,
                    TotalPrice = p.TotalPrice
                }).ToList(),
                Type = order.Type,
                InitialPrice = order.InitialPrice,
                EffectivePrice = order.EffectivePrice,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt
            };
        }

        public static RemoveOrderProductOutput ToRemoveOrderProductOutput(this Order order)
        {
            return new RemoveOrderProductOutput
            {
                Id = order.Id,
                Products = order.Products.Select(p => new RemoveOrderProductProductOutput
                {
                    Id = p.Id,
                    Name = p.Name,
                    Quantity = p.Quantity,
                    OriginalUnitPrice = p.OriginalUnitPrice,
                    EffectiveUnitPrice = p.EffectiveUnitPrice,
                    Rate = p.Rate,
                    Delta = p.Delta,
                    TotalPrice = p.TotalPrice
                }).ToList(),
                Type = order.Type,
                InitialPrice = order.InitialPrice,
                EffectivePrice = order.EffectivePrice,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt
            };
        }
    }
}