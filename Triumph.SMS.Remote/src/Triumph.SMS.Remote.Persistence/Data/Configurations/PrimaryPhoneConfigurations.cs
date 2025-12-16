using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Triumph.SMS.Remote.Core.ApplicationUsers;

namespace Triumph.SMS.Remote.Persistence.Data.Configurations;

public class PrimaryPhoneConfigurations : IEntityTypeConfiguration<PrimaryPhone>
{
    public void Configure(EntityTypeBuilder<PrimaryPhone> builder)
    {
        builder.HasKey(pp => pp.Id);

        builder.OwnsOne(pp => pp.Contact, contactBuilder =>
        {
            contactBuilder.Property(c => c.CountryCode)
                .HasMaxLength(5)
                .IsRequired();

            contactBuilder.Property(c => c.PhoneNumber)
                .HasMaxLength(15)
                .IsRequired();
        });
    }
}
