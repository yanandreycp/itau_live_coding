using FluentValidation.Results;
using LiveCoding.Domain.Enums;

namespace LiveCoding.Application.UseCases.CreateOrder
{
    public class CreateOrderValidation : ICreateOrderValidation
    {
        public Task<ValidationResult> ValidateAsync(CreateOrderInput input, CancellationToken cancellationToken)
        {
            var errors = new List<ValidationFailure>();

            if (input.Products is null || input.Products.Count == 0)
                errors.Add(new ValidationFailure("Products", "Products list cannot be null or empty"));

            if (input.Products is not null)
            {
                int i = 0;
                foreach (var product in input.Products)
                {
                    if (string.IsNullOrWhiteSpace(product.Name))
                        errors.Add(new ValidationFailure($"Products[{i}].Name", "Product name is required"));

                    if (product.Quantity <= 0)
                        errors.Add(new ValidationFailure($"Products[{i}].Quantity", "Quantity must be greater than zero"));

                    if (product.Price <= 0)
                        errors.Add(new ValidationFailure($"Products[{i}].Price", "Price must be greater than zero"));

                    i++;
                }
            }

            if (!Enum.IsDefined(typeof(EOrderType), input.Type))
                errors.Add(new ValidationFailure("Type", "Invalid order type"));

            return Task.FromResult(new ValidationResult(errors));
        }
    }
}