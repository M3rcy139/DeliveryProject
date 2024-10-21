using DeliveryProject.Persistence.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.Persistence.Configurations
{
    public class FilteredOrderConfiguration : IEntityTypeConfiguration<FilteredOrderEntity>
    {
        public void Configure(EntityTypeBuilder<FilteredOrderEntity> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.DeliveryTime)
                .IsRequired();

            builder.HasOne(f => f.Order)
                .WithMany()
                .HasForeignKey(f => f.OrderId);

            builder.HasOne(f => f.Area)
                .WithMany()
                .HasForeignKey(f => f.AreaId);
        }
    }
}
