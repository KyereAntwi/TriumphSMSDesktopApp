using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Triumph.SMS.Remote.Core.Payments;

namespace Triumph.SMS.Remote.Persistence.Data.Configurations;

public class PaymentHistoryConfiguration : IEntityTypeConfiguration<PaymentHistory>
{
    public void Configure(EntityTypeBuilder<PaymentHistory> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.PaymentType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.ReceivedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.PaidBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Currency)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(x => x.Amount)
            .IsRequired();

        builder.HasOne(x => x.Student)
            .WithMany()
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
