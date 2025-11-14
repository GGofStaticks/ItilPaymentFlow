using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Domain.Users;
using ItilPaymentFlow.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ItilPaymentFlow.Infrastructure.Persistence.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasColumnName("UserGuid")
                .HasColumnType("uuid")
                .ValueGeneratedNever();

            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
            builder.Property(u => u.PasswordHash).IsRequired();
            builder.Property(u => u.Role).HasMaxLength(100);

            builder.Property(u => u.FirstName).IsRequired().HasMaxLength(256);
            builder.Property(u => u.LastName).IsRequired().HasMaxLength(256);
            builder.Property(u => u.MiddleName).HasMaxLength(256);
        }
    }
}