using DeliveryProject.Business.Interfaces.Services;

namespace DeliveryProject.Business.Services;

public class DeliveryTimeCalculatorService : IDeliveryTimeCalculatorService
{
    private readonly Random _random = new();
    
    public DateTime CalculateDeliveryTime()
    {
        return new DateTime(2027, 5, 5, _random.Next(0, 24), _random.Next(0, 60), 0)
            .ToUniversalTime();
    }
}