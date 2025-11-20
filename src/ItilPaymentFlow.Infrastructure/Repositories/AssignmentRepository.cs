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
    internal sealed class AssignmentRepository : IAssignmentRepository
    {
        private readonly PaymentDbContext _context;
        public AssignmentRepository(PaymentDbContext context) => _context = context;

        public async Task AddAsync(Assignment assignment, CancellationToken cancellationToken = default)
            => await _context.Set<Assignment>().AddAsync(assignment, cancellationToken);

        public Task<Assignment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => _context.Set<Assignment>().FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        public async Task<List<Assignment>> ListActiveAsync(CancellationToken cancellationToken = default)
            => await _context.Set<Assignment>().Where(a => a.Active).OrderByDescending(a => a.CreatedAtUtc).ToListAsync(cancellationToken);

        public IQueryable<Assignment> Query() => _context.Set<Assignment>().AsQueryable();
    }
}
