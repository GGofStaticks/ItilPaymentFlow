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
    internal sealed class CompletedAssignmentConfiguration : IEntityTypeConfiguration<CompletedAssignment>
    {
        public void Configure(EntityTypeBuilder<CompletedAssignment> builder)
        {
            builder.ToTable("CompletedAssignments");
            builder.HasKey(ca => ca.Id);
            builder.Property(ca => ca.Id).HasColumnType("uuid").ValueGeneratedNever();
            builder.Property(ca => ca.AssignmentId).HasColumnType("uuid").IsRequired();
            builder.Property(ca => ca.UserId).HasColumnType("uuid").IsRequired();
            builder.Property(ca => ca.SubmissionText).HasColumnType("text");
            builder.Property(ca => ca.FileUrl).HasColumnType("text");
            builder.Property(ca => ca.Status).HasConversion<string>().HasMaxLength(32).HasColumnName("status");
            builder.Property(ca => ca.AwardedPoints).HasColumnName("awarded_points");
            builder.Property(ca => ca.CreatedAtUtc).HasColumnName("created_at_utc");
        }
    }
}
