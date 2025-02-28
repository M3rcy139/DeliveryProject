using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.Configurations
{
    public class PersonContactConfiguration : IEntityTypeConfiguration<PersonContactEntity>
    {
        public void Configure(EntityTypeBuilder<PersonContactEntity> builder)
        {
            builder.Property(s => s.PhoneNumber).IsRequired().HasMaxLength(20);

            builder.Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .HasOne(pc => pc.Person)
                .WithMany(p => p.Contacts)
                .HasForeignKey(pc => pc.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(pc => pc.Region)
                .WithMany(r => r.PersonContacts)
                .HasForeignKey(r => r.RegionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
