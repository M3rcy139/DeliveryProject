namespace DeliveryProject.Core.Models
{
    public class DeliverySlot : BaseUnit
    {
        public DateTime SlotTime { get; set; }

        public Guid DeliveryPersonId { get; set; }
        public DeliveryPerson DeliveryPerson { get; set; }
    }
}
