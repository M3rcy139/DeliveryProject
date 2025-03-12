using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DeliveryProject.DataAccess.Configurations
{
    public class PersonConfiguration : IEntityTypeConfiguration<PersonEntity>
    {
        public void Configure(EntityTypeBuilder<PersonEntity> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Status).IsRequired();

            builder
                .HasOne(p => p.Role)
                .WithMany();

            builder
                .HasOne(p => p.Region)
                .WithMany();

            builder
                .Property(p => p.Status)
                .HasConversion(new EnumToStringConverter<PersonStatus>());
        }
    }
}
