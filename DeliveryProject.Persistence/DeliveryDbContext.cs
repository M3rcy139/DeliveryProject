using DeliveryProject.Access.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.Access
{
    public class DeliveryDbContext(DbContextOptions<DeliveryDbContext> options) : DbContext(options)
    {
        public DbSet<AreaEntity> Areas { get; set; }
        public DbSet<FilteredOrderEntity> FilteredOrders { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
    }
}
