

namespace DeliveryProject.Access.Entities
{
    public class FilteredOrderEntity
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public int AreaId { get; set; }
        public DateTime DeliveryTime { get; set; }

        public OrderEntity Order { get; set; }
        public AreaEntity Area { get; set; }
    }
}
