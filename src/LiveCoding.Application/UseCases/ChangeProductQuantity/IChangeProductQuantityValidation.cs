using FluentValidation.Results;

namespace LiveCoding.Application.UseCases.ChangeProductQuantity
{
    public interface IChangeProductQuantityValidation
    {
        Task<ValidationResult> ValidateAsync(ChangeProductQuantityInput input, CancellationToken cancellationToken);
    }
}
