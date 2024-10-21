using Microsoft.EntityFrameworkCore;
using DeliveryProject.Persistence.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryProject.Persistence.Configurations
{
    public class AreaConfiguration : IEntityTypeConfiguration<AreaEntity>
    {
        public void Configure(EntityTypeBuilder<AreaEntity> builder)
        {
            builder.HasKey(a => a.Id);
                
            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
