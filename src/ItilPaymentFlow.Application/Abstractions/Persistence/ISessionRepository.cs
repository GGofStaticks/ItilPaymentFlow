using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Domain.Sessions;
using ItilPaymentFlow.Domain.ValueObjects;

namespace ItilPaymentFlow.Application.Abstractions.Persistence
{
    public interface ISessionRepository
    {
        Task Add(Session session);
        Task Update(Session session);
        Task Delete(Session session);

        Task<Session?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
        Task<Session?> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task DeleteAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

        Task<IEnumerable<Session>> GetAllActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task UpdateAsync(Session session, CancellationToken cancellationToken = default);
    }
}