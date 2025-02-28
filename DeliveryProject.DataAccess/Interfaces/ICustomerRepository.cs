using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.Interfaces
{
    public interface ICustomerRepository
    {
        Task<CustomerEntity?> GetCustomerByIdAsync(Guid customerId);
    }
}
