using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Domain.Assignments;

namespace ItilPaymentFlow.Application.Abstractions.Persistence
{
    public interface ILoyaltyRuleRepository
    {
        Task AddAsync(LoyaltyRule rule, CancellationToken cancellationToken = default);
        Task<List<LoyaltyRule>> ListAsync(CancellationToken cancellationToken = default);
        IQueryable<LoyaltyRule> Query();
        Task<LoyaltyRule?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}