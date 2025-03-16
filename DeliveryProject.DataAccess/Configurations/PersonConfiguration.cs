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
                .WithMany()
                .HasForeignKey(p => p.RoleId);

            builder
                .HasOne(p => p.Region)
                .WithMany()
                .HasForeignKey(p => p.RegionId);

            builder
                .HasMany(p => p.AttributeValues)
                .WithOne(av => av.Person)
                .HasForeignKey(p => p.PersonId);


            builder
                .Property(p => p.Status)
                .HasConversion(new EnumToStringConverter<PersonStatus>());

            builder.HasDiscriminator<int>("RoleId")
                .HasValue<CustomerEntity>((int)RoleType.Customer)
                .HasValue<DeliveryPersonEntity>((int)RoleType.DeliveryPerson)
                .HasValue<SupplierEntity>((int)RoleType.Supplier);
        }
    }
}
