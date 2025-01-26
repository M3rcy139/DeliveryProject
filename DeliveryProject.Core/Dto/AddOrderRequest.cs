namespace DeliveryProject.Core.Dto
{
    public struct AddOrderRequest
    {
        public int RegionId { get; set; }
        public double Weight { get; set; }
        public DateTime DeliveryTime { get; set; }
        public int SupplierId { get; set; }
    }
}
