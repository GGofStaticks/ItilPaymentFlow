using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Domain.Presale;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItilPaymentFlow.Infrastructure.Persistence.Configurations;

internal sealed class OfferConfiguration : IEntityTypeConfiguration<Offer>
{
    public void Configure(EntityTypeBuilder<Offer> builder)
    {
        builder.ToTable("Offers");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasColumnType("uuid")
            .ValueGeneratedNever();

        builder.Property(o => o.Number).HasMaxLength(64);
        builder.Property(o => o.Title).HasMaxLength(256);
        builder.Property(o => o.Amount).HasColumnType("numeric");
        builder.Property(o => o.ValidUntil).HasColumnName("valid_until");
        builder.Property(o => o.FileUrl).HasColumnType("text");
        builder.Property(o => o.SupplierId)
            .HasColumnName("supplier_id")
            .HasColumnType("uuid")
            .IsRequired();
        builder.Property(o => o.CreatedAtUtc).HasColumnName("created_at_utc");

        builder.Property(o => o.Status)
            .HasConversion<string>()
            .HasMaxLength(32)
            .HasColumnName("status");
    }
}