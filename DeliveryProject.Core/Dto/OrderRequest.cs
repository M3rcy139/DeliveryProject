namespace DeliveryProject.Core.Dto
{
    public class OrderRequest
    {
        public Guid? OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}
