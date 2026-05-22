using System.Diagnostics.CodeAnalysis;
using FluentValidation.Results;

namespace LiveCoding.Application.UseCases.GetOrder
{
    [ExcludeFromCodeCoverage]
    public class GetOrderValidation : IGetOrderValidation
    {
        public Task<ValidationResult> ValidateAsync(GetOrderInput input, CancellationToken cancellationToken)
        {
            var errors = new List<ValidationFailure>();

            if (input.Id == Guid.Empty)
                errors.Add(new ValidationFailure("Id", "Order Id is required"));

            return Task.FromResult(new ValidationResult(errors));
        }
    }
}
