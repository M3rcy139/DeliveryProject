using DeliveryProject.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess
{ 
    public class DeliveryDbContext : DbContext
    {
        public DeliveryDbContext(DbContextOptions<DeliveryDbContext> options) : base(options)
        {
        }
        public DbSet<RegionEntity> Regions { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<PersonEntity> Persons { get; set; }
        public DbSet<SupplierEntity> Suppliers { get; set; }
        public DbSet<DeliveryPersonEntity> DeliveryPersons { get; set; }
        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<OrderProductEntity> OrderProducts { get; set; }
        public DbSet<PersonContactEntity> PersonContacts { get; set; }
        public DbSet<DeliverySlotEntity> DeliverySlots { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<BatchUpload> BatchUploads { get; set; }
        public DbSet<UploadError> UploadErrors { get; set; }
        public DbSet<TempDeliveryPerson> TempDeliveryPersons { get; set; }
        public DbSet<TempPersonContact> TempPersonContacts { get; set; }
        public DbSet<TempDeliverySlot> TempDeliverySlots { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DeliveryDbContext).Assembly);
        }
    }
}
