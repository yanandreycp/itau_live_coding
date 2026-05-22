using System.Diagnostics.CodeAnalysis;
using FluentValidation.Results;

namespace LiveCoding.Application.UseCases.RemoveOrderProduct
{
    [ExcludeFromCodeCoverage]
    public class RemoveOrderProductValidation : IRemoveOrderProductValidation
    {
        public Task<ValidationResult> ValidateAsync(RemoveOrderProductInput input, CancellationToken cancellationToken)
        {
            var errors = new List<ValidationFailure>();

            if (input.OrderId == Guid.Empty)
                errors.Add(new ValidationFailure("OrderId", "Order Id is required"));

            if (input.ProductId == Guid.Empty)
                errors.Add(new ValidationFailure("ProductId", "Product Id is required"));

            return Task.FromResult(new ValidationResult(errors));
        }
    }
}
