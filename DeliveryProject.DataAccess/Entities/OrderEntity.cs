namespace DeliveryProject.DataAccess.Entities
{
    public class OrderEntity
    {
        public Guid Id { get; set; }
        public double Weight { get; set; }
        public int RegionId { get; set; }
        public DateTime DeliveryTime { get; set; }

        public RegionEntity Region { get; set; }
    }
}
