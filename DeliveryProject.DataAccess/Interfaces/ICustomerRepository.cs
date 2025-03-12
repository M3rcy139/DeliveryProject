using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.Interfaces
{
    public interface ICustomerRepository
    {
        Task<PersonEntity?> GetCustomerByIdAndRoleAsync(Guid personId);
    }
}
