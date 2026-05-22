using LiveCoding.Application.Generics;
using LiveCoding.Application.Interfaces;

namespace LiveCoding.Application.UseCases.RemoveOrderProduct
{
    public class RemoveOrderProductUseCase(IOrderService orderService, IRemoveOrderProductValidation validation) : IRemoveOrderProductUseCase
    {
        public async Task<Response<RemoveOrderProductOutput>> ExecuteAsync(RemoveOrderProductInput input, CancellationToken cancellationToken)
        {
            var validationResult = await validation.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                return new Response<RemoveOrderProductOutput>(false)
                    .AddError(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

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