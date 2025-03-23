using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DeliveryProject.DataAccess.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.Property(r => r.RoleType).IsRequired();

            builder
                .HasMany(r => r.RoleAttributes)
                .WithOne(p => p.Role)
                .HasForeignKey(p => p.RoleId);

            builder
                .Property(r => r.RoleType)
                .HasConversion(new EnumToStringConverter<RoleType>());
            
            builder.HasData(SeedDataRolesProvider.GetRoles());
        }
    }
}
