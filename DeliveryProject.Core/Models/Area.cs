
namespace DeliveryProject.Core.Models
{
    public class Area
    {
        public int Id { get; set; } 
        public string Name { get; set; } 

        public ICollection<Order> Orders { get; set; } 
    }
}
