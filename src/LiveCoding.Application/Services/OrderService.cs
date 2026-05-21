using LiveCoding.Application.Extensions.Mapping;
using LiveCoding.Application.Generics;
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
        public async Task<Response<GetOrderOutput>> GetOrderAsync(Guid orderId, CancellationToken cancellationToken)
        {
            try
            {
                var order = await orderRepository.GetOrderAsync(orderId, cancellationToken);
                if (order == null) return new Response<GetOrderOutput>(true);

                var response = order.ToGetOrderOutput();
                return new Response<GetOrderOutput>(true, response);
            }
            catch (Exception ex)
            {
                return new Response<GetOrderOutput>(false)
                    .AddError(ex.Message);
            }
        }

        public async Task<Response<CreateOrderOutput>> CreateOrderAsync(CreateOrderInput input, CancellationToken cancellationToken)
        {
            try
            {
                var order = input.ToOrder();
                order.CalculateOrderEffectivePrice();

                await orderRepository.SaveOrderAsync(order, cancellationToken);

                var createdOrder = await orderRepository.GetOrderAsync(order.Id, cancellationToken);
                var response = createdOrder.ToCreateOrderOutput();

                return new Response<CreateOrderOutput>(true, response);
            }
            catch (Exception ex)
            {
                return new Response<CreateOrderOutput>(false)
                    .AddError(ex.Message);
            }
        }

        public async Task<Response<ChangeProductQuantityOutput>> ChangeProductQuantityAsync(ChangeProductQuantityInput input, CancellationToken cancellationToken)
        {
            try
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
                var response = updatedOrder.ToChangeProductQuantityOutput();

                return new Response<ChangeProductQuantityOutput>(true, response);
            }
            catch (Exception ex)
            {
                return new Response<ChangeProductQuantityOutput>(false)
                    .AddError(ex.Message);
            }
        }

        public async Task<Response<RemoveOrderProductOutput>> RemoveOrderProductAsync(RemoveOrderProductInput input, CancellationToken cancellationToken)
        {
            try
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
                var response = updatedOrder.ToRemoveOrderProductOutput();

                return new Response<RemoveOrderProductOutput>(true, response);
            }
            catch (Exception ex)
            {
                return new Response<RemoveOrderProductOutput>(false)
                    .AddError(ex.Message);
            }
        }
    }
}