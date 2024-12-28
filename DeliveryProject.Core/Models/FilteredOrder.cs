
namespace DeliveryProject.Core.Models
{
    public class FilteredOrder
    {
        public Guid Id { get; set; } 
        public Guid OrderId { get; set; } 
        public int RegionId { get; set; } 
        public DateTime DeliveryTime { get; set; } 

        public Order Order { get; set; }
        public Region Region { get; set; } 
    }

}
