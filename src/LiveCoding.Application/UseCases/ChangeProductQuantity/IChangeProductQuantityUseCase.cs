namespace LiveCoding.Application.UseCases.ChangeProductQuantity
{
    public interface IChangeProductQuantityUseCase
    {
        Task<ChangeProductQuantityOutput> ExecuteAsync(ChangeProductQuantityInput input, CancellationToken cancellationToken);
    }
}