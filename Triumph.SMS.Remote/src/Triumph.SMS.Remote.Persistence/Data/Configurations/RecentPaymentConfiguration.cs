using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Triumph.SMS.Remote.Core.Students;

namespace Triumph.SMS.Remote.Persistence.Data.Configurations;

public class RecentPaymentConfiguration : IEntityTypeConfiguration<RecentPayment>
{
    public void Configure(EntityTypeBuilder<RecentPayment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.PaidBy)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(x => x.ReceivedBy)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasOne(x => x.PaymentType)
            .WithMany()
            .IsRequired();

        builder.OwnsOne(x => x.Amount, moneyBuilder => 
        {
            moneyBuilder.Property(m => m.Currency)
                .IsRequired()
                .HasMaxLength(3)
                .HasColumnName("AmountCurrency");

            moneyBuilder.Property(m => m.Amount)
                .IsRequired()
                .HasColumnName("AmountValue");
        });
    }
}
