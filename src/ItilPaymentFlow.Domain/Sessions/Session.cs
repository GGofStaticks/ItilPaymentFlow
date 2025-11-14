using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Domain.ValueObjects;

namespace ItilPaymentFlow.Domain.Sessions
{
    public sealed class Session
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid UserId { get; private set; }
        public string RefreshToken { get; private set; } = null!;
        public DateTime RefreshTokenExpiresAt { get; private set; }
        public DateTime StartedAtUtc { get; private set; }
        public DateTime? EndedAtUtc { get; private set; }

        private Session() { }

        private Session(Guid userId, string refreshToken, DateTime refreshTokenExpiresAt, DateTime startedAtUtc)
        {
            UserId = userId;
            RefreshToken = refreshToken;
            RefreshTokenExpiresAt = refreshTokenExpiresAt;
            StartedAtUtc = startedAtUtc;
        }

        public static Session Create(Guid userId, string refreshToken, DateTime refreshTokenExpiresAt, DateTime startedAtUtc)
        {
            return new Session(userId, refreshToken, refreshTokenExpiresAt, startedAtUtc);
        }

        public void EndSession()
        {
            EndedAtUtc = DateTime.UtcNow;
        }

        public void SetRefreshToken(string refreshToken, DateTime expiresAt)
        {
            RefreshToken = refreshToken;
            RefreshTokenExpiresAt = expiresAt;
        }
    }
}