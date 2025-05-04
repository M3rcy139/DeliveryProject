namespace DeliveryProject.Core.Dto
{
    public class OrderRequest
    {
        public Guid? Id { get; set; }
        public Guid CustomerId { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}
