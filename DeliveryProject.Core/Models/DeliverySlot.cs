namespace DeliveryProject.Core.Models
{
    public class DeliverySlot : Base
    {
        public DateTime SlotTime { get; set; }

        public Guid DeliveryPersonId { get; set; }
        public Person DeliveryPerson { get; set; }
    }
}
