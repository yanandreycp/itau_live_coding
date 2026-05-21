using LiveCoding.Application.Extensions.Mapping;
using LiveCoding.Application.Interfaces;
using LiveCoding.Application.UseCases.ChangeProductQuantity;
using LiveCoding.Application.UseCases.CreateOrder;
using LiveCoding.Application.UseCases.GetOrder;
using LiveCoding.Application.UseCases.RemoveOrderProduct;
using LiveCoding.Infrastructure.Interfaces;

namespace LiveCoding.Application.Services
{
    public class OrderService(IOrderRepository orderRepository) : IOrderService
    {
        public async Task<GetOrderOutput> GetOrderAsync(Guid orderId, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetOrderAsync(orderId, cancellationToken);
            if (order == null) throw new Exception("Order not found");

            return order.ToGetOrderOutput();
        }

        public async Task<CreateOrderOutput> CreateOrderAsync(CreateOrderInput input, CancellationToken cancellationToken)
        {
            var order = input.ToOrder();
            order.CalculateOrderEffectivePrice();

            await orderRepository.SaveOrderAsync(order, cancellationToken);

            var createdOrder = await orderRepository.GetOrderAsync(order.Id, cancellationToken);
            return createdOrder.ToCreateOrderOutput();
        }

        public async Task<ChangeProductQuantityOutput> ChangeProductQuantityAsync(ChangeProductQuantityInput input, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetOrderAsync(input.OrderId, cancellationToken);
            if (order == null) throw new Exception("Order not found");

            var existingProduct = order.Products?.FirstOrDefault(x => x.Id == input.ProductId);
            if (existingProduct != null)
            {
                existingProduct.Quantity = input.Quantity;
            }

            order.CalculateOrderEffectivePrice();
            order.UpdatedAt = DateTime.UtcNow;

            await orderRepository.UpdateOrderAsync(order, cancellationToken);

            var updatedOrder = await orderRepository.GetOrderAsync(order.Id, cancellationToken);
            return updatedOrder.ToChangeProductQuantityOutput();
        }

        public async Task<RemoveOrderProductOutput> RemoveOrderProductAsync(RemoveOrderProductInput input, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetOrderAsync(input.OrderId, cancellationToken);
            if (order == null) throw new Exception("Order not found");

            var existingProduct = order.Products?.FirstOrDefault(x => x.Id == input.ProductId);
            if (existingProduct != null)
            {
                order.Products?.Remove(existingProduct);
            }

            order.CalculateOrderEffectivePrice();
            order.UpdatedAt = DateTime.UtcNow;

            await orderRepository.UpdateOrderAsync(order, cancellationToken);

            var updatedOrder = await orderRepository.GetOrderAsync(order.Id, cancellationToken);
            return updatedOrder.ToRemoveOrderProductOutput();
        }
    }
}