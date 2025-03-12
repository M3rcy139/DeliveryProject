using DeliveryProject.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryProject.DataAccess.Configurations
{
    internal class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.Property(r => r.Role).IsRequired();

            builder
                .HasMany(r => r.Persons)
                .WithOne(p => p.Role)
                .HasForeignKey(p => p.RoleId);
        }
    }
}
