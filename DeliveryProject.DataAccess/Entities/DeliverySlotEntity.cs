namespace DeliveryProject.DataAccess.Entities
{
    public class DeliverySlotEntity : BaseEntity
    {
        public DateTime SlotTime { get; set; }

        public Guid DeliveryPersonId { get; set; }
        public PersonEntity DeliveryPerson { get; set; }
    }
}
