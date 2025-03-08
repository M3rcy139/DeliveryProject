namespace DeliveryProject.DataAccess.Entities
{
    public class DeliverySlotEntity
    {
        public Guid Id { get; set; }
        public DateTime SlotTime { get; set; }

        public Guid DeliveryPersonId { get; set; }
        public DeliveryPersonEntity DeliveryPerson { get; set; }
    }
}
