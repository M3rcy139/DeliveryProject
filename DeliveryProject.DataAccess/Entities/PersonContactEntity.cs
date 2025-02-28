namespace DeliveryProject.DataAccess.Entities
{
    public class PersonContactEntity : BaseUnitEntity
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public Guid PersonId { get; set; }
        public int RegionId { get; set; }
        public PersonEntity Person { get; set; }
        public RegionEntity Region { get; set; }
    }
}
