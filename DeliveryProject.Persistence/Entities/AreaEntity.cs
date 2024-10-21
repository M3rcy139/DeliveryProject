
namespace DeliveryProject.Persistence.Entities
{
    public class AreaEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<OrderEntity> Orders { get; set; }
    }
}
