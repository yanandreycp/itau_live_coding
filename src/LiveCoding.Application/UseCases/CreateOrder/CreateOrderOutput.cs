using LiveCoding.Domain.Enums;

namespace LiveCoding.Application.UseCases.CreateOrder
{
    public class CreateOrderOutput
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public EOrderType Type { get; set; }
    }
}