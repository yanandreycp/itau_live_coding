using FluentValidation.Results;

namespace LiveCoding.Application.UseCases.ChangeProductQuantity
{
    public class ChangeProductQuantityValidation : IChangeProductQuantityValidation
    {
        public Task<ValidationResult> ValidateAsync(ChangeProductQuantityInput input, CancellationToken cancellationToken)
        {
            var errors = new List<ValidationFailure>();

            if (input.OrderId == Guid.Empty)
                errors.Add(new ValidationFailure("OrderId", "Order Id is required"));

            if (input.ProductId == Guid.Empty)
                errors.Add(new ValidationFailure("ProductId", "Product Id is required"));

            if (input.Quantity <= 0)
                errors.Add(new ValidationFailure("Quantity", "Quantity must be greater than zero"));

            return Task.FromResult(new ValidationResult(errors));
        }
    }
}
