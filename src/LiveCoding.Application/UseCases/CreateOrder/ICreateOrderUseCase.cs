namespace LiveCoding.Application.UseCases.CreateOrder
{
    public interface ICreateOrderUseCase
    {
        Task<CreateOrderOutput> CreateOrderAsync(CreateOrderInput input, CancellationToken cancellation);
    }
}
