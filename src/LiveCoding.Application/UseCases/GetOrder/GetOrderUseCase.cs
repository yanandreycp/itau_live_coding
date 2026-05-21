namespace LiveCoding.Application.UseCases.GetOrder
{
    public class GetOrderUseCase : IGetOrderUseCase
    {
        public Task<GetOrderOutput> GetOrderAsync(GetOrderInput input, CancellationToken cancellation)
        {
            return Task.FromResult(new GetOrderOutput()
            {
                Id = Guid.NewGuid()
            });
        }
    }
}