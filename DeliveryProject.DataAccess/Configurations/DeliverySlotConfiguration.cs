using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.Configurations
{
    internal class DeliverySlotConfiguration : IEntityTypeConfiguration<DeliverySlotEntity>
    {
        public void Configure(EntityTypeBuilder<DeliverySlotEntity> builder)
        {
            builder.Property(ds => ds.SlotTime).IsRequired();
        }
    }
}
