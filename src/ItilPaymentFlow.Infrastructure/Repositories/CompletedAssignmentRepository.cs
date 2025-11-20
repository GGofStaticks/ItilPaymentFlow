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
    internal sealed class CompletedAssignmentRepository : ICompletedAssignmentRepository
    {
        private readonly PaymentDbContext _context;
        public CompletedAssignmentRepository(PaymentDbContext context) => _context = context;

        public async Task AddAsync(CompletedAssignment entity, CancellationToken cancellationToken = default)
            => await _context.Set<CompletedAssignment>().AddAsync(entity, cancellationToken);

        public Task<CompletedAssignment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => _context.Set<CompletedAssignment>().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        public async Task<List<CompletedAssignment>> ListByUserAsync(Guid userId, CancellationToken cancellationToken = default)
            => await _context.Set<CompletedAssignment>().Where(c => c.UserId == userId).OrderByDescending(c => c.CreatedAtUtc).ToListAsync(cancellationToken);

        public IQueryable<CompletedAssignment> Query() => _context.Set<CompletedAssignment>().AsQueryable();

        public async Task UpdateAsync(CompletedAssignment entity, CancellationToken cancellationToken = default)
        {
            _context.Set<CompletedAssignment>().Update(entity);
            await Task.CompletedTask;
        }
    }
}
