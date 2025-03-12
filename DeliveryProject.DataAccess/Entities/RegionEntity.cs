
namespace DeliveryProject.DataAccess.Entities
{
    public class RegionEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<PersonContactEntity> PersonContacts { get; set; } = new List<PersonContactEntity>();
    }
}
