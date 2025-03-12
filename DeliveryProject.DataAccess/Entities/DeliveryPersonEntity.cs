namespace DeliveryProject.DataAccess.Entities
{
    public class DeliveryPersonEntity : PersonEntity
    {
        public double Rating { get; set; }
        public ICollection<DeliverySlotEntity> DeliverySlots { get; set; } = new List<DeliverySlotEntity>();
    }
}
