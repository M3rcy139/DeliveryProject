namespace DeliveryProject.DataAccess.Entities
{
    public class DeliverySlotEntity : BaseUnitEntity
    {
        public DateTime SlotTime { get; set; }

        public Guid DeliveryPersonId { get; set; }
        public DeliveryPersonEntity DeliveryPerson { get; set; }
    }
}
