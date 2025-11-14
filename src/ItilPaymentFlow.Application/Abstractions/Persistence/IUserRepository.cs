using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Domain.Users;

namespace ItilPaymentFlow.Application.Abstractions.Persistence
{
    public interface IUserRepository
    {
        Task Add(User user);
        Task Update(User user);
        Task Delete(User user);
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    }
}