namespace DeliveryProject.DataAccess.Entities
{
    public class TempDeliverySlot
    {
        public Guid Id { get; set; } 
        public DateTime SlotTime { get; set; }

        public Guid DeliveryPersonId { get; set; } 
        public TempDeliveryPerson DeliveryPerson { get; set; } 
    }
}
