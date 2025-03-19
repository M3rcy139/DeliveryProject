namespace DeliveryProject.Core.Models
{
    public class DeliverySlot
    {
        public Guid Id { get; set; }
        public DateTime SlotTime { get; set; }

        public Guid DeliveryPersonId { get; set; }
        public Person DeliveryPerson { get; set; }
    }
}
