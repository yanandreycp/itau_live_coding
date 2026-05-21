using LiveCoding.Application.UseCases.CreateOrder;
using LiveCoding.Application.UseCases.GetOrder;
using Microsoft.AspNetCore.Mvc;

namespace LiveCoding.WebApi.Controllers
{
    public class OrderController(
        IGetOrderUseCase getOrderUseCase,
        ICreateOrderUseCase createOrderUseCase) : Controller
    {

        [HttpGet("orders/{id}")]
        public async Task<IActionResult> GetOrder([FromRoute] Guid id, CancellationToken cancellation)
        {
            var input = new GetOrderInput { Id = id };

            var response = await getOrderUseCase.GetOrderAsync(input, cancellation);
            return Ok(response);
        }

        [HttpPost("orders")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderInput input, CancellationToken cancellation)
        {
            var response = await createOrderUseCase.CreateOrderAsync(input, cancellation);
            return Ok(response);
        }
    }
}