using DeliveryProject.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DeliveryProject.DataAccess.Configurations
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<InvoiceEntity>
    {
        public void Configure(EntityTypeBuilder<InvoiceEntity> builder)
        {
            builder.HasKey(i => i.Id);
            
            builder.Property(i => i.DeliveryTime).IsRequired();
            builder.Property(i => i.IsExecuted).IsRequired();

            builder
                .HasOne(i => i.Order)
                .WithOne()
                .HasForeignKey<InvoiceEntity>(i => i.OrderId);

            builder
                .HasOne(i => i.DeliveryPerson)
                .WithMany();
        }
    }
}
