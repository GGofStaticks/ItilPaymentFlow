using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Domain.Assignments;

namespace ItilPaymentFlow.Application.Abstractions.Persistence
{
    public interface IAssignmentRepository
    {
        Task AddAsync(Assignment assignment, CancellationToken cancellationToken = default);
        Task<Assignment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Assignment>> ListActiveAsync(CancellationToken cancellationToken = default);
        IQueryable<Assignment> Query();
    }
}
