using DeliveryProject.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess
{ 
    public class DeliveryDbContext(DbContextOptions<DeliveryDbContext> options) : DbContext(options)
    {
        public DbSet<RegionEntity> Regions { get; set; }
        public DbSet<FilteredOrderEntity> FilteredOrders { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DeliveryDbContext).Assembly);
        }
    }
}
