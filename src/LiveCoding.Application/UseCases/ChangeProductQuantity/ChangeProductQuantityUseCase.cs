using LiveCoding.Application.Generics;
using LiveCoding.Application.Interfaces;

namespace LiveCoding.Application.UseCases.ChangeProductQuantity
{
    public class ChangeProductQuantityUseCase(IOrderService orderService, IChangeProductQuantityValidation validation) : IChangeProductQuantityUseCase
    {
        public async Task<Response<ChangeProductQuantityOutput>> ExecuteAsync(ChangeProductQuantityInput input, CancellationToken cancellationToken)
        {
            var validationResult = await validation.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                return new Response<ChangeProductQuantityOutput>(false)
                    .AddError(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            try
            {
                var response = await orderService.ChangeProductQuantityAsync(input, cancellationToken);
                return new Response<ChangeProductQuantityOutput>(true, response.Content);
            }
            catch (Exception ex)
            {
                return new Response<ChangeProductQuantityOutput>(false)
                    .AddError(ex.Message);
            }
        }
    }
}