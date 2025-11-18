using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Domain.Presale;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItilPaymentFlow.Infrastructure.Persistence.Configurations;

internal sealed class ContractConfiguration : IEntityTypeConfiguration<Contract>
{
    public void Configure(EntityTypeBuilder<Contract> builder)
    {
        builder.ToTable("Contracts");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnType("uuid")
            .ValueGeneratedNever();

        builder.Property(c => c.Title).HasMaxLength(256);
        builder.Property(c => c.Number).HasMaxLength(64);
        builder.Property(c => c.StartAt).HasColumnName("start_at");
        builder.Property(c => c.EndAt).HasColumnName("end_at");
        builder.Property(c => c.FileUrl).HasColumnType("text");
        builder.Property(c => c.CounterpartyId)
            .HasColumnName("counterparty_id")
            .HasColumnType("uuid")
            .IsRequired();
        builder.Property(c => c.CreatedAtUtc).HasColumnName("created_at_utc");
    }
}