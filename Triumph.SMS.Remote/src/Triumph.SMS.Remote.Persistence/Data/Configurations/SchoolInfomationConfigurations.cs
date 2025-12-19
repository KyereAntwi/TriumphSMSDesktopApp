using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Triumph.SMS.Remote.Core.SchoolConfigs;

namespace Triumph.SMS.Remote.Persistence.Data.Configurations;

public class SchoolInfomationConfigurations : IEntityTypeConfiguration<SchoolInformation>
{
    public void Configure(EntityTypeBuilder<SchoolInformation> builder)
    {
        builder.ToTable("SchoolInformation");

        builder.HasKey(si => si.Id);

        builder.Property(si => si.Name)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(si => si.Logo)
            .HasMaxLength(500);

        builder.Property(si => si.Address)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(si => si.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property (si => si.Email)
            .HasMaxLength(225);

        builder.Property(si => si.Website)
            .HasMaxLength(225);

        builder.Property(si => si.Motto)
            .IsRequired()
            .HasMaxLength(300);

        builder.HasMany(si => si.Lisences)
            .WithOne(l => l.School!)
            .HasForeignKey(l => l.SchoolId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
