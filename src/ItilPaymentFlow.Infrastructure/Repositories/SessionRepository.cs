using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Abstractions.Persistence;
using ItilPaymentFlow.Domain.Sessions;
using ItilPaymentFlow.Domain.ValueObjects;
using ItilPaymentFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ItilPaymentFlow.Infrastructure.Repositories
{
    internal sealed class SessionRepository : ISessionRepository
    {
        private readonly PaymentDbContext _context;
        public SessionRepository(PaymentDbContext context) => _context = context;

        public async Task Add(Session session)
        {
            await _context.Set<Session>().AddAsync(session);
        }

        public Task Update(Session session)
        {
            _context.Set<Session>().Update(session);
            return Task.CompletedTask;
        }

        public async Task UpdateAsync(Session session, CancellationToken cancellationToken = default)
        {
            _context.Set<Session>().Update(session);
            await Task.CompletedTask;
        }

        public Task Delete(Session session)
        {
            _context.Set<Session>().Remove(session);
            return Task.CompletedTask;
        }

        public Task<Session?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
            => _context.Set<Session>().AsNoTracking().FirstOrDefaultAsync(s => s.RefreshToken == refreshToken, cancellationToken);

        public Task<Session?> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
            => _context.Set<Session>().AsNoTracking().FirstOrDefaultAsync(s => s.UserId == userId && s.EndedAtUtc == null, cancellationToken);

        public async Task<IEnumerable<Session>> GetAllActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Sessions
                .Where(s => s.UserId == userId && s.EndedAtUtc == null)
                .ToListAsync(cancellationToken);
        }

        public async Task DeleteAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var sessions = await _context.Set<Session>().Where(s => s.UserId == userId).ToListAsync(cancellationToken);
            _context.Set<Session>().RemoveRange(sessions);
            await Task.CompletedTask;
        }
    }
}