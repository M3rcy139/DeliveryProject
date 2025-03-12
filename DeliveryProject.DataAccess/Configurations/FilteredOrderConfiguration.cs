using DeliveryProject.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess.Configurations
{
    public class FilteredOrderConfiguration : IEntityTypeConfiguration<FilteredOrderEntity>
    {
        public void Configure(EntityTypeBuilder<FilteredOrderEntity> builder)
        {
            builder.HasKey(f => f.Id);

            builder.HasOne(f => f.Order)
                .WithMany()
                .HasForeignKey(f => f.OrderId);
        }
    }
}
