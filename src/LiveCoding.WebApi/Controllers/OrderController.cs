using LiveCoding.Application.UseCases.ChangeProductQuantity;
using LiveCoding.Application.UseCases.CreateOrder;
using LiveCoding.Application.UseCases.GetOrder;
using LiveCoding.Application.UseCases.RemoveOrderProduct;
using Microsoft.AspNetCore.Mvc;

namespace LiveCoding.WebApi.Controllers
{
    public class OrderController(
        IGetOrderUseCase getOrderUseCase,
        ICreateOrderUseCase createOrderUseCase,
        IChangeProductQuantityUseCase changeProductQuantityUseCase,
        IRemoveOrderProductUseCase removeOrderProductUseCase) : Controller
    {

        [HttpGet("orders/{id}")]
        public async Task<IActionResult> GetOrder([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var input = new GetOrderInput { Id = id };

            var response = await getOrderUseCase.ExecuteAsync(input, cancellationToken);
            return Ok(response);
        }

        [HttpPost("orders")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderInput input, CancellationToken cancellationToken)
        {
            var response = await createOrderUseCase.ExecuteAsync(input, cancellationToken);
            return Ok(response);
        }

        [HttpPut("orders/{orderId}/items/{itemId}")]
        public async Task<IActionResult> ChangeProductQuantity(
            [FromRoute] Guid orderId,
            [FromRoute] Guid itemId,
            [FromBody] ChangeProductQuantityInput body, CancellationToken cancellationToken)
        {
            var input = new ChangeProductQuantityInput(orderId, itemId)
            {
                Quantity = body.Quantity
            };

            var response = await changeProductQuantityUseCase.ExecuteAsync(input, cancellationToken);
            return Ok(response);
        }

        [HttpDelete("orders/{orderId}/items/{itemId}")]
        public async Task<IActionResult> RemoveOrderProduct(
            [FromRoute] Guid orderId,
            [FromRoute] Guid itemId,
            CancellationToken cancellationToken)
        {
            var input = new RemoveOrderProductInput(orderId, itemId);
            var response = await removeOrderProductUseCase.ExecuteAsync(input, cancellationToken);
            return Ok(response);
        }
    }
}