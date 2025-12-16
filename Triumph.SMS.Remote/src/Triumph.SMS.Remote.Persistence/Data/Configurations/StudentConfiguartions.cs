using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Triumph.SMS.Remote.Core.Students;

namespace Triumph.SMS.Remote.Persistence.Data.Configurations;

public class StudentConfiguartions : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(x => x.OtherNames)
            .HasMaxLength(100);

        builder.Property(x => x.DateOfBirth)
            .IsRequired();
        
        builder.Property(x => x.Residence)
            .HasMaxLength(200);
        
        builder.HasMany(x => x.ParentPhones)
            .WithOne(x => x.Student!)
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.RecentPayment)
            .WithOne(x => x.Student);
    }
}
