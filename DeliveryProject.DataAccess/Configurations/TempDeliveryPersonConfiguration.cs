using DeliveryProject.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryProject.DataAccess.Configurations
{
    public class TempDeliveryPersonConfiguration : IEntityTypeConfiguration<TempDeliveryPerson>
    {
        public void Configure(EntityTypeBuilder<TempDeliveryPerson> builder)
        {
            builder.Property(tdp => tdp.Name).IsRequired().HasMaxLength(100);
            builder.Property(tdp => tdp.Rating).HasDefaultValue(0);

            builder
                .HasMany(tdp => tdp.Contacts)
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
