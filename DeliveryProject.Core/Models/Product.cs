namespace DeliveryProject.Core.Models
{
    public class Product : Base
    {
        public string Name { get; set; }
        public double Weight { get; set; }
        public decimal Price { get; set; }
        public Guid SupplierId { get; set; }
        
    }
}
