using LiveCoding.Application.Generics;

namespace LiveCoding.Application.UseCases.ChangeProductQuantity
{
    public interface IChangeProductQuantityUseCase
    {
        Task<Response<ChangeProductQuantityOutput>> ExecuteAsync(ChangeProductQuantityInput input, CancellationToken cancellationToken);
    }
}