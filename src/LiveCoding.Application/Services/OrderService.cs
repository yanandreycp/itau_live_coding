using LiveCoding.Application.Extensions.Mapping;
using LiveCoding.Application.Generics;
using LiveCoding.Application.Interfaces;
using LiveCoding.Application.UseCases.ChangeProductQuantity;
using LiveCoding.Application.UseCases.CreateOrder;
using LiveCoding.Application.UseCases.GetOrder;
using LiveCoding.Application.UseCases.RemoveOrderProduct;
using LiveCoding.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace LiveCoding.Application.Services
{
    public class OrderService(IOrderRepository orderRepository, ILogger<OrderService> logger) : IOrderService
    {
        public async Task<Response<GetOrderOutput>> GetOrderAsync(Guid orderId, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("[OrderService][GetOrderAsync] Getting order {OrderId}", orderId);

                var order = await orderRepository.GetOrderAsync(orderId, cancellationToken);
                if (order == null) return new Response<GetOrderOutput>(true);

                var response = order.ToGetOrderOutput();
                return new Response<GetOrderOutput>(true, response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[OrderService][GetOrderAsync] Failed to get order {OrderId}", orderId);
                return new Response<GetOrderOutput>(false)
                    .AddError(ex.Message);
            }
        }

        public async Task<Response<CreateOrderOutput>> CreateOrderAsync(CreateOrderInput input, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("[OrderService][CreateOrderAsync] Creating order with {ProductCount} products of type {Type}",
                    input.Products?.Count, input.Type);

                var order = input.ToOrder();
                order.CalculateOrderEffectivePrice();

                await orderRepository.SaveOrderAsync(order, cancellationToken);

                var createdOrder = await orderRepository.GetOrderAsync(order.Id, cancellationToken);
                var response = createdOrder.ToCreateOrderOutput();

                logger.LogInformation("[OrderService][CreateOrderAsync] Order {OrderId} created successfully", order.Id);
                return new Response<CreateOrderOutput>(true, response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[OrderService][CreateOrderAsync] Failed to create order");
                return new Response<CreateOrderOutput>(false)
                    .AddError(ex.Message);
            }
        }

        public async Task<Response<ChangeProductQuantityOutput>> ChangeProductQuantityAsync(ChangeProductQuantityInput input, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("[OrderService][ChangeProductQuantityAsync] Changing product {ProductId} to quantity {Quantity} in order {OrderId}",
                    input.ProductId, input.Quantity, input.OrderId);

                var order = await orderRepository.GetOrderAsync(input.OrderId, cancellationToken);
                if (order == null)
                {
                    return new Response<ChangeProductQuantityOutput>(false)
                        .AddError("Order not found");
                }

                var existingProduct = order.Products?.FirstOrDefault(x => x.Id == input.ProductId);
                if (existingProduct == null)
                {
                    return new Response<ChangeProductQuantityOutput>(false)
                        .AddError("Product not found");
                }

                existingProduct.Quantity = input.Quantity;
                order.CalculateOrderEffectivePrice();
                order.UpdatedAt = DateTime.UtcNow;

                await orderRepository.UpdateOrderAsync(order, cancellationToken);

                var updatedOrder = await orderRepository.GetOrderAsync(order.Id, cancellationToken);
                var response = updatedOrder.ToChangeProductQuantityOutput();

                logger.LogInformation("[OrderService][ChangeProductQuantityAsync] Product {ProductId} quantity updated to {Quantity}", input.ProductId, input.Quantity);
                return new Response<ChangeProductQuantityOutput>(true, response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[OrderService][ChangeProductQuantityAsync] Failed to change product quantity");
                return new Response<ChangeProductQuantityOutput>(false)
                    .AddError(ex.Message);
            }
        }

        public async Task<Response<RemoveOrderProductOutput>> RemoveOrderProductAsync(RemoveOrderProductInput input, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("[OrderService][RemoveOrderProductAsync] Removing product {ProductId} from order {OrderId}",
                    input.ProductId, input.OrderId);

                var order = await orderRepository.GetOrderAsync(input.OrderId, cancellationToken);
                if (order == null)
                {
                    return new Response<RemoveOrderProductOutput>(false)
                        .AddError("Order not found");
                }

                var existingProduct = order.Products?.FirstOrDefault(x => x.Id == input.ProductId);
                if (existingProduct == null)
                {
                    return new Response<RemoveOrderProductOutput>(false)
                        .AddError("Product not found");
                }

                order.Products?.Remove(existingProduct);
                order.CalculateOrderEffectivePrice();
                order.UpdatedAt = DateTime.UtcNow;

                await orderRepository.UpdateOrderAsync(order, cancellationToken);

                var updatedOrder = await orderRepository.GetOrderAsync(order.Id, cancellationToken);
                var response = updatedOrder.ToRemoveOrderProductOutput();

                logger.LogInformation("[OrderService][RemoveOrderProductAsync] Product {ProductId} removed from order {OrderId}", input.ProductId, input.OrderId);
                return new Response<RemoveOrderProductOutput>(true, response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[OrderService][RemoveOrderProductAsync] Failed to remove product");
                return new Response<RemoveOrderProductOutput>(false)
                    .AddError(ex.Message);
            }
        }
    }
}