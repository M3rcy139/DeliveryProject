
namespace DeliveryProject.Core.Models
{
    public class Order
    {
        public Guid Id { get; set; } 
        public double Weight { get; set; } 
        public int AreaId { get; set; } 
        public DateTime DeliveryTime { get; set; } 

        public Area Area { get; set; } 
    }
}
