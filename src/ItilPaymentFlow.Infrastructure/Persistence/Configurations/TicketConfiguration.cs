using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Domain.Tickets;
using ItilPaymentFlow.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ItilPaymentFlow.Infrastructure.Persistence.Configurations
{
    internal sealed class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.ToTable("Tickets");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .HasColumnType("uuid")
                .ValueGeneratedNever();

            builder.Property(t => t.Number).HasMaxLength(32);
            builder.Property(t => t.Title).HasMaxLength(256);
            builder.Property(t => t.Priority);
            builder.Property(t => t.Description).HasColumnType("text");
            builder.Property(t => t.Contacts).HasMaxLength(256);
            builder.Property(t => t.Attachments).HasColumnType("text");
            builder.Property(t => t.CreatedAtUtc).HasColumnName("created_at_utc");
            builder.Property(t => t.SlaTime).HasColumnName("sla_time");
            builder.Property(t => t.Status).HasConversion<string>().HasMaxLength(32).HasColumnName("Status");

            builder.Property(t => t.AuthorId)
                .HasColumnName("AuthorId")
                .HasColumnType("uuid")
                .IsRequired();

            builder.HasOne(t => t.Author)
                .WithMany(u => u.Tickets)
                .HasForeignKey(t => t.AuthorId)
                .HasPrincipalKey(u => u.Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}