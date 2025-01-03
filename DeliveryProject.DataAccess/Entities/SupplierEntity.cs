namespace DeliveryProject.DataAccess.Entities
{
    public class SupplierEntity : ParticipantEntity
    {
        public ICollection<OrderEntity> OrdersSupplied { get; set; } = new List<OrderEntity>();
        public string Email { get; set; }
    }
}

