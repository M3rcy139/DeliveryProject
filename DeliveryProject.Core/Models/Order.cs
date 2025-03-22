using DeliveryProject.Core.Enums;

namespace DeliveryProject.Core.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public OrderStatus Status { get; set; }

        public ICollection<OrderPerson> OrderPersons { get; set; } = new List<OrderPerson>();
        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
        public Invoice Invoice { get; set; }
    }
}

