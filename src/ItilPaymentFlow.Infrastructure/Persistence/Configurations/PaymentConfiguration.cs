using ItilPaymentFlow.Domain.Payments;
using ItilPaymentFlow.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItilPaymentFlow.Infrastructure.Persistence.Configurations;

internal sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payments");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasConversion(id => id.Value, value => PaymentId.From(value))
            .ValueGeneratedNever();

        builder.Property(p => p.Reference)
            .HasMaxLength(64)
            .IsRequired();

        builder.OwnsOne(p => p.Amount, amount =>
        {
            amount.Property(a => a.Amount).HasColumnName("amount").HasPrecision(18, 2);
            amount.Property(a => a.Currency).HasColumnName("currency").HasMaxLength(3);
        });

        builder.Property(p => p.Status)
            .HasConversion<string>()
            .HasMaxLength(16)
            .IsRequired();

        builder.Property(p => p.CreatedAtUtc).HasColumnName("created_at_utc");
        builder.Property(p => p.CompletedAtUtc).HasColumnName("completed_at_utc");
        builder.Property(p => p.FailureReason).HasMaxLength(256).HasColumnName("failure_reason");
    }
}

