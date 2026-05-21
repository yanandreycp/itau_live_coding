using LiveCoding.Application.Generics;
using LiveCoding.Application.Interfaces;

namespace LiveCoding.Application.UseCases.RemoveOrderProduct
{
    public class RemoveOrderProductUseCase(IOrderService orderService) : IRemoveOrderProductUseCase
    {
        public async Task<Response<RemoveOrderProductOutput>> ExecuteAsync(RemoveOrderProductInput input, CancellationToken cancellationToken)
        {
            try
            {
                var response = await orderService.RemoveOrderProductAsync(input, cancellationToken);
                return new Response<RemoveOrderProductOutput>(true, response.Content);
            }
            catch (Exception ex)
            {
                return new Response<RemoveOrderProductOutput>(false)
                    .AddError(ex.Message);
            }
        }
    }
}