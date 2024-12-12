namespace DeliveryProject.Application.Dto
{
    public record AddOrderRequest
    (
        int AreaId,
        double Weight,
        DateTime DeliveryTime
    );
}
