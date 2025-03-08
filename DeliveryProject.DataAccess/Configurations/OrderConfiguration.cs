using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.CreatedTime)
                .IsRequired();

            builder.Property(o => o.Status)
                .IsRequired();

            builder
                .HasMany(o => o.OrderPersons)
                .WithOne(op => op.Order)
                .HasForeignKey(op => op.OrderId);

            builder
                .HasMany(o => o.OrderProducts)
                .WithOne(op => op.Order)
                .HasForeignKey(op => op.OrderId);

            builder
                .HasOne(o => o.Invoice)
                .WithOne(i => i.Order)
                .HasForeignKey<InvoiceEntity>(i => i.OrderId);
        }
    }
}
