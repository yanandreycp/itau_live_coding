using LiveCoding.Domain.Enums;

namespace LiveCoding.Application.UseCases.CreateOrder
{
    public class CreateOrderInput
    {
        public List<CreateOrderProductInput> Products { get; set; }
        public EOrderType Type { get; set; }
    }

    public class CreateOrderProductInput
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}