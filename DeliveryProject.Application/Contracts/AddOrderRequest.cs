
namespace DeliveryProject.Application.Contracts
{
    public record AddOrderRequest
    (
        int Weight,
        int AreaId,
        DateTime DeliveryTime
    );
}
