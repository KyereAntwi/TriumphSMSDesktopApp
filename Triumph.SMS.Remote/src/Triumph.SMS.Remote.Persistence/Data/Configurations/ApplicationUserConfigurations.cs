using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Triumph.SMS.Remote.Core.ApplicationUsers;

namespace Triumph.SMS.Remote.Persistence.Data.Configurations;

public class ApplicationUserConfigurations : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasKey(au => au.Id);

        builder.Property(au => au.Username)
            .IsRequired()
            .HasMaxLength(322);

        builder.Property(au => au.Email)
            .HasMaxLength(322);

        builder.Property(au => au.HashedPassword)
            .IsRequired()
            .HasMaxLength(512);

        builder.HasMany(au => au.Contacts)
            .WithOne()
            .HasForeignKey("ApplicationUserId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
