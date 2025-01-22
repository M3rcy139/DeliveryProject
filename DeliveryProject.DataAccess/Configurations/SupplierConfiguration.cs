using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.Configurations
{
    public class SupplierConfiguration : IEntityTypeConfiguration<SupplierEntity>
    {
        public void Configure(EntityTypeBuilder<SupplierEntity> builder)
        {
            builder.ToTable("Suppliers");

            builder.Property(s => s.Id).IsRequired();
            builder.Property(s => s.Name).IsRequired().HasMaxLength(100);
            builder.Property(s => s.PhoneNumber).IsRequired().HasMaxLength(20);
            builder.Property(s => s.Rating).HasDefaultValue(0);

            builder.Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder
            .HasMany(s => s.OrdersSupplied)
            .WithOne(o => o.Supplier)
            .HasForeignKey(o => o.SupplierId);
        }
    }
}
