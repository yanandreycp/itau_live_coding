using LiveCoding.Domain.Enums;

namespace LiveCoding.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public List<Product> Products { get; set; }
        public EOrderType Type { get; set; }
        public decimal InitialPrice { get; set; }
        public decimal EffectivePrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public void CalculateOrderEffectivePrice()
        {
            if (Products == null || Products.Count == 0)
            {
                InitialPrice = 0m;
                EffectivePrice = 0m;
                return;
            }

            InitialPrice = Products.Sum(p => p.OriginalUnitPrice * p.Quantity);

            foreach (var prod in Products)
            {
                prod.CalculateProductEffectivePrice(Type);
            }

            EffectivePrice = Products.Sum(p => p.TotalPrice);
        }
    }
}