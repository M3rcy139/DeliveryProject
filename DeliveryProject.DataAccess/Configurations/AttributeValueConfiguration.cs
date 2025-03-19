using DeliveryProject.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess.Configurations
{
    public class AttributeValueConfiguration : IEntityTypeConfiguration<AttributeValueEntity>
    {
        public void Configure(EntityTypeBuilder<AttributeValueEntity> builder)
        {
            builder.HasKey(av => av.Id);

            builder.Property(av => av.Value).IsRequired();

            builder
                .HasOne(av => av.Person)
                .WithMany()
                .HasForeignKey(av => av.PersonId);

            builder
                .HasOne(av => av.Attribute)
                .WithMany(a => a.AttributeValues)
                .HasForeignKey(av => av.AttributeId);
        }
    }
}
