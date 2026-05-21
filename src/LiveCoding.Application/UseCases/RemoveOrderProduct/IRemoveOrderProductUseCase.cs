using LiveCoding.Application.Generics;

namespace LiveCoding.Application.UseCases.RemoveOrderProduct
{
    public interface IRemoveOrderProductUseCase
    {
        Task<Response<RemoveOrderProductOutput>> ExecuteAsync(RemoveOrderProductInput input, CancellationToken cancellationToken);
    }
}