using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly DeliveryDbContext _context;

        public SupplierRepository(DeliveryDbContext context)
        {
            _context = context;
        }

        public async Task<SupplierEntity?> GetByIdAsync(int id)
        {
            return await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}

