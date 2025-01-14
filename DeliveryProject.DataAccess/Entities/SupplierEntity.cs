namespace DeliveryProject.DataAccess.Entities
{
    public class SupplierEntity : BaseUnitEntity
    {
        public ICollection<OrderEntity> OrdersSupplied { get; set; } = new List<OrderEntity>();
        public string Email { get; set; }
    }
}

