using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Triumph.SMS.Remote.Core.SchoolConfigs;

namespace Triumph.SMS.Remote.Persistence.Data.Configurations;

public class LisenceConfigurations : IEntityTypeConfiguration<Lisence>
{
    public void Configure(EntityTypeBuilder<Lisence> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.LisenceKey)
            .IsRequired()
            .HasMaxLength(12);
    }
}
