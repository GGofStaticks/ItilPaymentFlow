using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Domain.Sessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


namespace ItilPaymentFlow.Infrastructure.Persistence.Configurations
{
    internal sealed class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable("Sessions");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id).HasColumnType("uuid").ValueGeneratedNever();
            builder.Property(s => s.UserId).HasColumnName("UserId").HasColumnType("uuid").IsRequired();
            builder.Property(s => s.StartedAtUtc).HasColumnName("started_at_utc").IsRequired();
            builder.Property(s => s.EndedAtUtc).HasColumnName("ended_at_utc");
            builder.Property(s => s.RefreshToken).HasColumnName("RefreshToken").HasColumnType("text");
            builder.Property(s => s.RefreshTokenExpiresAt).HasColumnName("refresh_token_expires_at");
            builder.HasIndex(s => s.RefreshToken);
            builder.HasIndex(s => s.UserId);
        }
    }
}