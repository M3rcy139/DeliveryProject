namespace DeliveryProject.DataAccess.Entities
{
    public class ProductEntity : BaseEntity
    {
        public string Name { get; set; }
        public double Weight { get; set; }
        public decimal Price { get; set; }
        public Guid SupplierId { get; set; }
    }
}