namespace DeliveryProject.DataAccess.Entities
{
    public class ProductEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Weight { get; set; }
        public decimal Price { get; set; }
        public Guid SupplierId { get; set; }
        public SupplierEntity Supplier { get; set; }
    }
}
