using DeliveryProject.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess.Configurations
{
    public class OrderPersonConfiguration : IEntityTypeConfiguration<OrderPersonEntity>
    {
        public void Configure(EntityTypeBuilder<OrderPersonEntity> builder)
        {
            builder.HasKey(op => new { op.OrderId, op.PersonId });

            builder
                .HasOne(op => op.Order)
                .WithMany(o => o.OrderPersons)
                .HasForeignKey(op => op.OrderId);

            builder
                .HasOne(op => op.Person)
                .WithMany()
                .HasForeignKey(op => op.PersonId);
        }
    }
}
