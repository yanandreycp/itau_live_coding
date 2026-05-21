using LiveCoding.Application.Generics;

namespace LiveCoding.Application.UseCases.CreateOrder
{
    public interface ICreateOrderUseCase
    {
        Task<Response<CreateOrderOutput>> ExecuteAsync(CreateOrderInput input, CancellationToken cancellationToken);
    }
}
