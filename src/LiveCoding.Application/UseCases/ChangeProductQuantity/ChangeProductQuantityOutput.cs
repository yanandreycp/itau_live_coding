using LiveCoding.Domain.Enums;

namespace LiveCoding.Application.UseCases.ChangeProductQuantity
{
    public class ChangeProductQuantityOutput
    {
        public Guid Id { get; set; }
        public List<ChangeProductQuantityProductOutput> Products { get; set; }
        public EOrderType Type { get; set; }
        public decimal InitialPrice { get; set; }
        public decimal EffectivePrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class ChangeProductQuantityProductOutput
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal OriginalUnitPrice { get; set; }
        public decimal EffectiveUnitPrice { get; set; }
        public decimal Rate { get; set; }
        public decimal Delta { get; set; }
        public decimal TotalPrice { get; set; }
    }
}