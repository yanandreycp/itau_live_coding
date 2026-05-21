namespace LiveCoding.Application.UseCases.RemoveOrderProduct
{
    public interface IRemoveOrderProductUseCase
    {
        Task<RemoveOrderProductOutput> ExecuteAsync(RemoveOrderProductInput input, CancellationToken cancellationToken);
    }
}