namespace DeliveryProject.Core.Models
{
    public class DeliveryPerson : Person
    {
        public double Rating { get; set; }
        public ICollection<DeliverySlot> DeliverySlots { get; set; } = new List<DeliverySlot>();
    }
}
