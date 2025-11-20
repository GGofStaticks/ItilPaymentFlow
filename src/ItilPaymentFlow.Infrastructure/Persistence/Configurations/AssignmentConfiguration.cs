using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Domain.Assignments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItilPaymentFlow.Infrastructure.Persistence.Configurations
{
    internal sealed class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> builder)
        {
            builder.ToTable("Assignments");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).HasColumnType("uuid").ValueGeneratedNever();
            builder.Property(a => a.Title).HasMaxLength(256);
            builder.Property(a => a.ShortDescription).HasMaxLength(1024).HasColumnType("text");
            builder.Property(a => a.Points);
            builder.Property(a => a.Active).HasColumnName("active");
            builder.Property(a => a.CreatedAtUtc).HasColumnName("created_at_utc");
        }
    }
}
