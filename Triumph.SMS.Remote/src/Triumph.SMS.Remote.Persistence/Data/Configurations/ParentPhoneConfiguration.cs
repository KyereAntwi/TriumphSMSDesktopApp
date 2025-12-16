using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Triumph.SMS.Remote.Core.Common.Enums;
using Triumph.SMS.Remote.Core.Students;

namespace Triumph.SMS.Remote.Persistence.Data.Configurations;

public class ParentPhoneConfiguration : IEntityTypeConfiguration<ParentPhone>
{
    public void Configure(EntityTypeBuilder<ParentPhone> builder)
    {
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.Contact, contactBuilder =>
        {
            contactBuilder.Property(c => c.PhoneNumber)
                .IsRequired()
                .HasMaxLength(15)
                .HasColumnName("PhoneNumber");
            
            contactBuilder.Property(c => c.CountryCode)
                .IsRequired()
                .HasMaxLength(4)
                .HasColumnName("CountryCode");
        });

        builder.Property(x => x.Type)
            .HasConversion(v => (int)v, v => (ContactType)v)
            .IsRequired();

        builder.HasOne(x => x.Student)
            .WithMany(s => s.ParentPhones)
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}