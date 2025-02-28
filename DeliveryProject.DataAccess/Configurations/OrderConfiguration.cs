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

            builder.Property(o => o.DeliveryTime)
                .IsRequired();

            builder.Property(o => o.Amount)
                .IsRequired();

            builder
                .HasMany(o => o.Persons)
                .WithMany(p => p.Orders);

            builder
                .HasMany(o => o.Products)
                .WithMany();
        }
    }
}
