using System.Text.Json.Serialization;

namespace LiveCoding.Application.UseCases.ChangeProductQuantity
{
    public class ChangeProductQuantityInput(Guid orderId, Guid productId)
    {
        [JsonIgnore]
        public Guid OrderId { get; set; } = orderId;

        [JsonIgnore]
        public Guid ProductId { get; set; } = productId;

        public int Quantity { get; set; }
    }
}