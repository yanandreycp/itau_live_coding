namespace LiveCoding.Application.UseCases.CreateOrder
{
    public interface ICreateOrderUseCase
    {
        Task<CreateOrderOutput> ExecuteAsync(CreateOrderInput input, CancellationToken cancellationToken);
    }
}
