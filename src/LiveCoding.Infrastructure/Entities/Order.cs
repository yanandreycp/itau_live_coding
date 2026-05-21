using LiveCoding.Domain.Enums;
using System;

namespace LiveCoding.Infrastructure.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public EOrderType OrderType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}