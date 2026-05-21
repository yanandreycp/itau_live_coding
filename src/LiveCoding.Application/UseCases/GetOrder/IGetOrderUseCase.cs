namespace LiveCoding.Application.UseCases.GetOrder
{
    public interface IGetOrderUseCase
    {
        Task<GetOrderOutput> ExecuteAsync(GetOrderInput input, CancellationToken cancellationToken);
    }
}