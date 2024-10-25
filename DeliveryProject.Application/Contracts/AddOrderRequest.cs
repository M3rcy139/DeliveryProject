namespace DeliveryProject.Application.Contracts
{
    public record AddOrderRequest
    (
        int AreaId,
        double Weight,
        DateTime DeliveryTime
    );
}
