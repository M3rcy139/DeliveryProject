namespace DeliveryProject.DataAccess.Entities
{
    public class DeliveryPersonEntity : BaseUnitEntity
    {
        public ICollection<OrderEntity> OrdersDelivered { get; set; } = new List<OrderEntity>();
        public List<DateTime> DeliverySlots { get; set; } = new();
    }
}
