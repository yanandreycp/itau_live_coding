namespace LiveCoding.Domain.Entities
{
    using LiveCoding.Domain.Enums;

    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal OriginalUnitPrice { get; set; }
        public decimal EffectiveUnitPrice { get; set; }
        public decimal Rate { get; set; }
        public decimal Delta { get; set; }
        public decimal TotalPrice { get; set; }

        public void CalculateProductEffectivePrice(EOrderType orderType)
        {
            decimal baseTotal = OriginalUnitPrice * Quantity;

            switch (orderType)
            {
                case EOrderType.Express:
                    Rate = 0.15m;
                    Delta = baseTotal * Rate;
                    EffectiveUnitPrice = OriginalUnitPrice * (1 + Rate);
                    TotalPrice = baseTotal * (1 + Rate);
                    break;
                case EOrderType.Subscription:
                    Rate = -0.10m;
                    Delta = baseTotal * Rate;
                    EffectiveUnitPrice = OriginalUnitPrice * (1 + Rate);
                    TotalPrice = baseTotal * (1 + Rate);
                    break;
                default:
                    Rate = 0m;
                    Delta = baseTotal * Rate;
                    EffectiveUnitPrice = OriginalUnitPrice * (1 + Rate);
                    TotalPrice = baseTotal * (1 + Rate);
                    break;
            }
        }
    }
}