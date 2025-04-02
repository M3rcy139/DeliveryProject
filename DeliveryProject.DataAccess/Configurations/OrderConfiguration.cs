using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeliveryProject.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using DeliveryProject.Core.Enums;

namespace DeliveryProject.DataAccess.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Amount).IsRequired();
            builder.Property(o => o.CreatedTime).IsRequired();
            builder.Property(o => o.Status).IsRequired();

            builder
                .HasMany(o => o.OrderPersons)
                .WithOne(op => op.Order)
                .HasForeignKey(op => op.OrderId);

            builder
                .HasMany(o => o.OrderProducts)
                .WithOne(op => op.Order)
                .HasForeignKey(op => op.OrderId);
            

            builder
                .Property(o => o.Status)
                .HasConversion(new EnumToStringConverter<OrderStatus>());
        }
    }
}
