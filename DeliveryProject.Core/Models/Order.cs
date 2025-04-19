using DeliveryProject.Core.Enums;

namespace DeliveryProject.Core.Models
{
    public class Order : BaseModel
    {
        public DateTime CreatedTime { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Amount { get; set; }

        public ICollection<OrderPerson> OrderPersons { get; set; } = new List<OrderPerson>();
        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    }
}

