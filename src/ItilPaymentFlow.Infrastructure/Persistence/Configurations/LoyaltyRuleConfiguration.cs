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
    internal sealed class LoyaltyRuleConfiguration : IEntityTypeConfiguration<LoyaltyRule>
    {
        public void Configure(EntityTypeBuilder<LoyaltyRule> builder)
        {
            builder.ToTable("LoyaltyRules");
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).HasColumnType("uuid").ValueGeneratedNever();
            builder.Property(r => r.Name).HasMaxLength(256);
            builder.Property(r => r.PointsForAssignmentType).HasColumnName("points_for_assignment_type");
            builder.Property(r => r.LevelThreshold).HasColumnName("level_threshold");
            builder.Property(r => r.CreatedAtUtc).HasColumnName("created_at_utc");
        }
    }
}
