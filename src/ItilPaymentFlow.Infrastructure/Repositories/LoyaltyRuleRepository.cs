using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Abstractions.Persistence;
using ItilPaymentFlow.Domain.Assignments;
using ItilPaymentFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ItilPaymentFlow.Infrastructure.Repositories
{
    internal sealed class LoyaltyRuleRepository : ILoyaltyRuleRepository
    {
        private readonly PaymentDbContext _context;
        public LoyaltyRuleRepository(PaymentDbContext context) => _context = context;

        public async Task AddAsync(LoyaltyRule rule, CancellationToken cancellationToken = default)
            => await _context.Set<LoyaltyRule>().AddAsync(rule, cancellationToken);

        public async Task<LoyaltyRule?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await _context.Set<LoyaltyRule>()
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        public async Task<List<LoyaltyRule>> ListAsync(CancellationToken cancellationToken = default)
            => await _context.Set<LoyaltyRule>()
                .OrderBy(r => r.LevelThreshold)
                .ToListAsync(cancellationToken);

        public IQueryable<LoyaltyRule> Query()
            => _context.Set<LoyaltyRule>().AsQueryable();
    }
}