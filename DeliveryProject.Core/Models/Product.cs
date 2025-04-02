namespace DeliveryProject.Core.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Weight { get; set; }
        public decimal Price { get; set; }
        public Guid SupplierId { get; set; }
        
    }
}
