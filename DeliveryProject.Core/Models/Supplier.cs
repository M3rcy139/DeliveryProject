namespace DeliveryProject.Core.Models
{
    public class Supplier : Person
    {
        public double Rating { get; set; }
        public ICollection<Product> Products { get; } = new List<Product>();
    }
}
