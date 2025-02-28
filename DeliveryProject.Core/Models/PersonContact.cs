namespace DeliveryProject.Core.Models
{
    public class PersonContact : BaseUnit
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public Guid PersonId { get; set; }
        public int RegionId { get; set; }
        public Person Person { get; set; }
        public Region Region { get; set; }
    }
}
