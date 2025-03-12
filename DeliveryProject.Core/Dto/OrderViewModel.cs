namespace DeliveryProject.Core.Dto
{
    public class OrderViewModel
    {
        public Guid? OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public List<ProductItemViewModel> Products { get; set; }
        public DateTime DeliveryTime { get; set; }
    }
}
