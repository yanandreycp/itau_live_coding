namespace LiveCoding.Application.UseCases.GetOrder
{
    public interface IGetOrderUseCase
    {
        Task<GetOrderOutput> GetOrderAsync(GetOrderInput input, CancellationToken cancellation);
    }
}