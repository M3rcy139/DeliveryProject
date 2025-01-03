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

            builder.Property(dp => dp.Id).IsRequired();
            builder.Property(dp => dp.Name).IsRequired().HasMaxLength(100);
            builder.Property(dp => dp.PhoneNumber).IsRequired().HasMaxLength(20);
            builder.Property(dp => dp.Rating).HasDefaultValue(0);

            builder
                .Property(dp => dp.DeliverySlots)
                .HasConversion(
                    v => string.Join(',', v.Select(d => d.ToString("o"))),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(DateTime.Parse).ToList()
                )
                .HasColumnType("text");

            builder
            .HasMany(dp => dp.OrdersDelivered)
            .WithOne(o => o.DeliveryPerson)
            .HasForeignKey(o => o.DeliveryPersonId);
        }
    }
}
