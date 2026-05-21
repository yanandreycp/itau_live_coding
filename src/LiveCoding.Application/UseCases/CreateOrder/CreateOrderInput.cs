using LiveCoding.Domain.Enums;

namespace LiveCoding.Application.UseCases.CreateOrder
{
    public class CreateOrderInput
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public EOrderType Type { get; set; }
    }
}