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

            builder.Property(o => o.Weight)
                .IsRequired();

            builder.Property(o => o.DeliveryTime)
                .IsRequired();

            builder.HasOne(o => o.Region)
                .WithMany(a => a.Orders)
                .HasForeignKey(o => o.RegionId);
        }
    }
}
