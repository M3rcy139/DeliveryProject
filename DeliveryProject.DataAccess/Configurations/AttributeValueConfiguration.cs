using DeliveryProject.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess.Configurations
{
    public class AttributeValueConfiguration : IEntityTypeConfiguration<AttributeValueEntity>
    {
        public void Configure(EntityTypeBuilder<AttributeValueEntity> builder)
        {
            builder.HasKey(pav => pav.PersonId);

            builder.Property(pav => pav.Value).IsRequired();

            builder
                .HasOne(pav => pav.Person)
                .WithMany(p => p.PersonAttributeValues)
                .HasForeignKey(pav => pav.PersonId);

            builder
                .HasOne(pav => pav.Attribute)
                .WithMany(a => a.AttributeValues)
                .HasForeignKey(pav => pav.AttributeId);
        }
    }
}
