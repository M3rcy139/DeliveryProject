using DeliveryProject.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess
{ 
    public class DeliveryDbContext : DbContext
    {
        public DeliveryDbContext() { }

        public DeliveryDbContext(DbContextOptions<DeliveryDbContext> options) : base(options)
        {
        }
        public DbSet<RegionEntity> Regions { get; set; }
        public DbSet<FilteredOrderEntity> FilteredOrders { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<SupplierEntity> Suppliers { get; set; }
        public DbSet<DeliveryPersonEntity> DeliveryPersons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<BaseUnitEntity>();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DeliveryDbContext).Assembly);
        }
    }
}
