using Microsoft.EntityFrameworkCore;
using DeliveryProject.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryProject.DataAccess.Configurations
{
    public class RegionConfiguration : IEntityTypeConfiguration<RegionEntity>
    {
        public void Configure(EntityTypeBuilder<RegionEntity> builder)
        {
            builder.HasKey(r => r.Id);
                
            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(120);

            builder.HasMany(r => r.Orders)
                .WithOne(o => o.Region)
                .HasForeignKey(o => o.RegionId);
        }
    }
}
