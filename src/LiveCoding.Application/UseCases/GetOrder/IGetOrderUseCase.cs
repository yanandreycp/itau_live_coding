using LiveCoding.Application.Generics;

namespace LiveCoding.Application.UseCases.GetOrder
{
    public interface IGetOrderUseCase
    {
        Task<Response<GetOrderOutput>> ExecuteAsync(GetOrderInput input, CancellationToken cancellationToken);
    }
}