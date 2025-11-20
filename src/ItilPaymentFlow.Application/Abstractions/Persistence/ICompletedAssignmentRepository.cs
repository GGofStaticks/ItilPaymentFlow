using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Domain.Assignments;

namespace ItilPaymentFlow.Application.Abstractions.Persistence
{
    public interface ICompletedAssignmentRepository
    {
        Task AddAsync(CompletedAssignment entity, CancellationToken cancellationToken = default);
        Task<CompletedAssignment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<CompletedAssignment>> ListByUserAsync(Guid userId, CancellationToken cancellationToken = default);
        IQueryable<CompletedAssignment> Query();
        Task UpdateAsync(CompletedAssignment entity, CancellationToken cancellationToken = default);
    }
}
