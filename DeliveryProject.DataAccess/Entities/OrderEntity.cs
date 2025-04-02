using DeliveryProject.Core.Enums;

namespace DeliveryProject.DataAccess.Entities
{
    public class OrderEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Amount { get; set; }

        public ICollection<OrderPersonEntity> OrderPersons { get; set; } = new List<OrderPersonEntity>();
        public ICollection<OrderProductEntity> OrderProducts { get; set; } = new List<OrderProductEntity>();
    }
}
