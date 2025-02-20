namespace DeliveryProject.Core.Models
{
    public class TempDeliveryPerson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public double Rating { get; set; }
        public List<DateTime> DeliverySlots { get; set; } = new();
    }
}
