namespace DeliveryProject.API.Dto
{
    public class AddOrderRequest
    {
        public int regionId { get; set; }
        public double Weight { get; set; }
        public DateTime DeliveryTime { get; set; }
    }
}
