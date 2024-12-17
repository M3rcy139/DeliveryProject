

namespace DeliveryProject.DataAccess.Entities
{
    public class FilteredOrderEntity
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public int RegionId { get; set; }
        public DateTime DeliveryTime { get; set; }

        public OrderEntity Order { get; set; }
        public RegionEntity Region { get; set; }
    }
}
