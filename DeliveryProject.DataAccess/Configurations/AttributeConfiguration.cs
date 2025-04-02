using DeliveryProject.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.DataProviders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DeliveryProject.DataAccess.Configurations
{
    public class AttributeConfiguration : IEntityTypeConfiguration<AttributeEntity>
    {
        public void Configure(EntityTypeBuilder<AttributeEntity> builder)
        {
            builder.HasKey(a => a.Id);

            builder
                .HasMany(a => a.AttributeValues)
                .WithOne(pa => pa.Attribute)
                .HasForeignKey(pa => pa.AttributeId);
            
            builder
                .HasMany(a => a.RoleAttributes)
                .WithOne(pa => pa.Attribute)
                .HasForeignKey(pa => pa.AttributeId);

            builder
                .Property(a => a.Key)
                .HasConversion(new EnumToStringConverter<AttributeKey>());

            builder
                .Property(a => a.Type)
                .HasConversion(new EnumToStringConverter<AttributeType>());

            builder.HasData(AttributesDataProvider.GetAttributes());
        }
    }
}
