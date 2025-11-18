using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Domain.Presale;

namespace ItilPaymentFlow.Application.Abstractions.Persistence;

public interface IContractRepository
{
    Task AddAsync(Contract contract, CancellationToken cancellationToken = default);
    Task<Contract?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Contract>> ListAsync(CancellationToken cancellationToken = default);
    IQueryable<Contract> Query();
}