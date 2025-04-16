namespace DeliveryProject.DataAccess.Entities
{
    public class TempDeliveryPerson : BaseEntity
    {
        public int RegionId { get; set; }
        public int RoleId { get; set; }

        public ICollection<TempAttributeValue> AttributeValues { get; set; } = new List<TempAttributeValue>();
        public ICollection<TempDeliverySlot> DeliverySlots { get; set; } = new List<TempDeliverySlot>();
    }
}
