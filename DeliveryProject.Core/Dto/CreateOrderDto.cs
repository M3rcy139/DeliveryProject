namespace DeliveryProject.Core.Dto;

public class CreateOrderDto
{
    public Guid CustomerId { get; set; }
    public List<ProductDto> Products { get; set; }
}