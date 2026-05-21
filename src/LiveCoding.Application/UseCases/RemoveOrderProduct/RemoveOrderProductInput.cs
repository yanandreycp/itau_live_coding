using System.Text.Json.Serialization;

namespace LiveCoding.Application.UseCases.RemoveOrderProduct
{
    public class RemoveOrderProductInput(Guid orderId, Guid productId)
    {
        [JsonIgnore]
        public Guid OrderId { get; set; } = orderId;

        [JsonIgnore]
        public Guid ProductId { get; set; } = productId;
    }
}