using FluentValidation.Results;

namespace LiveCoding.Application.UseCases.RemoveOrderProduct
{
    public interface IRemoveOrderProductValidation
    {
        Task<ValidationResult> ValidateAsync(RemoveOrderProductInput input, CancellationToken cancellationToken);
    }
}
