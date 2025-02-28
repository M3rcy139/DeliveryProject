
namespace DeliveryProject.Core.Models
{
    public class Region
    {
        public int Id { get; set; } 
        public string Name { get; set; } 

        public ICollection<PersonContact> PersonContacts { get; set; } = new List<PersonContact>();
    }
}
