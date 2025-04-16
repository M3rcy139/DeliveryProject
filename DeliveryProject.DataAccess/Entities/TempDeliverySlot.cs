namespace DeliveryProject.DataAccess.Entities
{
    public class TempDeliverySlot : BaseEntity
    {
        public Guid DeliveryPersonId { get; set; }
        public DateTime SlotTime { get; set; }

        public TempDeliveryPerson DeliveryPerson { get; set; }  
    }
}
