namespace DeliveryProject.Core.Dto
{
    public class UpdateOrderRequest
    {
        public Guid OrderId { get; set; }
        public int RegionId { get; set; }
        public double Weight { get; set; }
        public DateTime DeliveryTime { get; set; }
        public int SupplierId { get; set; }
    }
}
