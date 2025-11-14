using ItilPaymentFlow.Domain.Abstractions;
using ItilPaymentFlow.Domain.Payments;
using ItilPaymentFlow.Domain.Sessions;
using ItilPaymentFlow.Domain.Tickets;
using ItilPaymentFlow.Domain.Users;
using ItilPaymentFlow.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;



namespace ItilPaymentFlow.Infrastructure.Persistence
{
    public sealed class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options) { }

        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Session> Sessions => Set<Session>();
        public DbSet<Ticket> Tickets => Set<Ticket>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // конфигурации из assembly (Payment, User, Ticket, Session)
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentDbContext).Assembly);

            modelBuilder.Entity<User>(builder =>
            {
                builder.HasKey(u => u.Id);
                builder.Property(u => u.Id)
                    .HasColumnName("UserGuid")
                    .HasColumnType("uuid")
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<Ticket>(builder =>
            {
                builder.HasKey(t => t.Id);
                builder.Property(t => t.Id).HasColumnType("uuid").ValueGeneratedNever();

                builder.Property(t => t.AuthorId)
                    .HasColumnName("AuthorId")
                    .HasColumnType("uuid")
                    .IsRequired();

                builder.HasOne(t => t.Author)
                    .WithMany(u => u.Tickets)
                    .HasForeignKey(t => t.AuthorId)
                    .HasPrincipalKey(u => u.Id)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Session>(builder =>
            {
                builder.HasKey(s => s.Id);

                builder.Property(s => s.Id).HasColumnType("uuid").ValueGeneratedNever();

                builder.Property(s => s.UserId)
                    .HasColumnName("UserId")
                    .HasColumnType("uuid")
                    .IsRequired();

                builder.Property(s => s.RefreshToken).HasColumnName("RefreshToken").HasColumnType("text");
                builder.Property(s => s.RefreshTokenExpiresAt).HasColumnName("refresh_token_expires_at");
                builder.Property(s => s.StartedAtUtc).HasColumnName("started_at_utc");
                builder.Property(s => s.EndedAtUtc).HasColumnName("ended_at_utc");
            });
        }
    }
}