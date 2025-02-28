using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.Configurations
{
    public class DeliveryPersonConfiguration : IEntityTypeConfiguration<DeliveryPersonEntity>
    {
        public void Configure(EntityTypeBuilder<DeliveryPersonEntity> builder)
        {
            builder.ToTable("DeliveryPersons");
            builder.Property(dp => dp.Rating).HasDefaultValue(0);

            builder
                .HasMany(d => d.DeliverySlots)
                .WithOne(ds => ds.DeliveryPerson)
                .HasForeignKey(ds => ds.DeliveryPersonId);
        }
    }
}
