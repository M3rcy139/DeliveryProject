using DeliveryProject.DataAccess.Entities;
using DeliveryProject.DataAccess.SeedData;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess.Configurations
{
    public class RoleAttributeConfiguration : IEntityTypeConfiguration<RoleAttributeEntity>
    {
        public void Configure(EntityTypeBuilder<RoleAttributeEntity> builder)
        {
            builder.HasKey(rpa => new { rpa.RoleId, rpa.AttributeId });

            builder
                .HasOne(rpa => rpa.Attribute)
                .WithMany(a => a.RoleAttributes)
                .HasForeignKey(rpa => rpa.AttributeId);

            builder
                .HasOne(rpa => rpa.Role)
                .WithMany(r => r.RoleAttributes)
                .HasForeignKey(rpa => rpa.RoleId);

            builder.HasData(SeedDataRoleAttributesProvider.GetRoleAttributes());
        }
    }
}
