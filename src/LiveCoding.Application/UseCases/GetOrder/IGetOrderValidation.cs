using FluentValidation.Results;

namespace LiveCoding.Application.UseCases.GetOrder
{
    public interface IGetOrderValidation
    {
        Task<ValidationResult> ValidateAsync(GetOrderInput input, CancellationToken cancellationToken);
    }
}
