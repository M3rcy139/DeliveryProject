using DeliveryProject.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryProject.DataAccess.Configurations
{
    public class TempDeliveryPersonConfiguration : IEntityTypeConfiguration<TempDeliveryPerson>
    {
        public void Configure(EntityTypeBuilder<TempDeliveryPerson> builder)
        {
            builder
                .HasMany(tdp => tdp.AttributeValues)
                .WithOne(tpc => tpc.DeliveryPerson)
                .HasForeignKey(tpc => tpc.DeliveryPersonId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(tdp => tdp.DeliverySlots)
                .WithOne(tds => tds.DeliveryPerson)
                .HasForeignKey(tds => tds.DeliveryPersonId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
