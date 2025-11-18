using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Domain.Presale;

namespace ItilPaymentFlow.Application.Abstractions.Persistence;

public interface IMeetingRepository
{
    Task AddAsync(Meeting meeting, CancellationToken cancellationToken = default);
    Task<Meeting?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Meeting>> ListAsync(CancellationToken cancellationToken = default);
    IQueryable<Meeting> Query();
}