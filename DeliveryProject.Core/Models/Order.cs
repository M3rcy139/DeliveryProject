
namespace DeliveryProject.Core.Models
{
    public class Order
    {
        public Guid Id { get; set; } 
        public double Weight { get; set; } 
        public int RegionId { get; set; } 
        public DateTime DeliveryTime { get; set; } 

        public Region Region { get; set; } 
    }
}
