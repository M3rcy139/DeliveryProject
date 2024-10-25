namespace DeliveryProject.Persistence.Entities
{
    public class OrderEntity
    {
        public Guid Id { get; set; }
        public double Weight { get; set; }
        public int AreaId { get; set; }
        public DateTime DeliveryTime { get; set; }

        public AreaEntity Area { get; set; }
    }
}
