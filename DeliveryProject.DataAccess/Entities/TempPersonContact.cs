namespace DeliveryProject.DataAccess.Entities
{
    public class TempPersonContact
    {
        public Guid Id { get; set; } 
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int RegionId { get; set; }

        public Guid DeliveryPersonId { get; set; } 
        public TempDeliveryPerson DeliveryPerson { get; set; } 
    }
}
