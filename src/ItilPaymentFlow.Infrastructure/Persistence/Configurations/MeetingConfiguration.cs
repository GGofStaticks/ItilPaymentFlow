using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Domain.Presale;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItilPaymentFlow.Infrastructure.Persistence.Configurations;

internal sealed class MeetingConfiguration : IEntityTypeConfiguration<Meeting>
{
    public void Configure(EntityTypeBuilder<Meeting> builder)
    {
        builder.ToTable("Meetings");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasColumnType("uuid")
            .ValueGeneratedNever();

        builder.Property(m => m.At).HasColumnName("at");
        builder.Property(m => m.Topic).HasMaxLength(256);
        builder.Property(m => m.Participants).HasMaxLength(512);
        builder.Property(m => m.FileUrl).HasColumnType("text");
        builder.Property(m => m.Link).HasColumnType("text");
        builder.Property(m => m.OrganiserId)
            .HasColumnName("organiser_id")
            .HasColumnType("uuid")
            .IsRequired();
        builder.Property(m => m.CreatedAtUtc).HasColumnName("created_at_utc");
    }
}