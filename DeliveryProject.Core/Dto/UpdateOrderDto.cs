namespace DeliveryProject.Core.Dto;

public class UpdateOrderDto
{
    public Guid Id { get; set; }
    public List<ProductDto> Products { get; set; }
}