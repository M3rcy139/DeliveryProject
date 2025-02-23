using DeliveryProject.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryProject.DataAccess.Configurations
{
    public class TempDeliveryPersonConfiguration : IEntityTypeConfiguration<TempDeliveryPerson>
    {
        public void Configure(EntityTypeBuilder<TempDeliveryPerson> builder)
        {
            builder.ToTable("TempDeliveryPersons");

            builder.Property(tdp => tdp.Id).IsRequired();
            builder.Property(tdp => tdp.Name).IsRequired().HasMaxLength(100);
            builder.Property(tdp => tdp.PhoneNumber).IsRequired().HasMaxLength(20);
            builder.Property(tdp => tdp.Rating).HasDefaultValue(0);

            builder
                .Property(tdp => tdp.DeliverySlots)
                .HasConversion(
                    v => string.Join(',', v.Select(d => d.ToString("o"))),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(DateTime.Parse).ToList()
                )
                .HasColumnType("text");
        }
    }
}
