namespace DeliveryProject.DataAccess.Entities
{
    public class TempDeliveryPerson
    {
        public Guid Id { get; set; } 
        public string Name { get; set; }
        public double Rating { get; set; }

        public ICollection<TempPersonContact> Contacts { get; set; } = new List<TempPersonContact>();
        public ICollection<TempDeliverySlot> DeliverySlots { get; set; } = new List<TempDeliverySlot>();
    }
}
