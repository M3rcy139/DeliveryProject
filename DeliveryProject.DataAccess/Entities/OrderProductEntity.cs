namespace DeliveryProject.DataAccess.Entities
{
    public class OrderProductEntity
    {
        public int Quantity { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }

        public ProductEntity Product { get; set; }
        public OrderEntity Order { get; set; }
    }
}
