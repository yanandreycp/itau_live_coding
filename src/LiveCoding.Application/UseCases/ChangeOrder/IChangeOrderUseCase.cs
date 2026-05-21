namespace LiveCoding.Application.UseCases.ChangeOrder
{
    public interface IChangeOrderUseCase
    {
        Task<ChangeOrderOutput> ChangeOrderAsync(ChangeOrderInput input, CancellationToken cancellation);
    }
}
