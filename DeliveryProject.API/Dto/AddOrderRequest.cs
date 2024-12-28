namespace DeliveryProject.API.Dto
{
    public class AddOrderRequest
    {
        public int RegionId { get; set; }
        public double Weight { get; set; }
        public DateTime DeliveryTime { get; set; }
    }
}
