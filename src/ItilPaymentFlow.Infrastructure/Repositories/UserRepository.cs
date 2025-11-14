using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Abstractions.Persistence;
using ItilPaymentFlow.Domain.Users;
using ItilPaymentFlow.Domain.ValueObjects;
using ItilPaymentFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ItilPaymentFlow.Infrastructure.Persistence.Repositories
{
    internal sealed class UserRepository : IUserRepository
    {
        private readonly PaymentDbContext _context;
        public UserRepository(PaymentDbContext context) => _context = context;

        public async Task Add(User user) => await _context.Users.AddAsync(user);

        public async Task Update(User user)
        {
            _context.Users.Update(user);
            await Task.CompletedTask;
        }

        public async Task Delete(User user)
        {
            _context.Users.Remove(user);
            await Task.CompletedTask;
        }

        public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default) =>
            _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        public async Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            var session = await _context.Sessions.AsNoTracking().FirstOrDefaultAsync(s => s.RefreshToken == refreshToken, cancellationToken);
            if (session == null) return null;
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == session.UserId, cancellationToken);
        }
    }
}