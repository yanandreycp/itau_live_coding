using FluentValidation.Results;

namespace LiveCoding.Application.UseCases.CreateOrder
{
    public interface ICreateOrderValidation
    {
        Task<ValidationResult> ValidateAsync(CreateOrderInput input, CancellationToken cancellationToken);
    }
}