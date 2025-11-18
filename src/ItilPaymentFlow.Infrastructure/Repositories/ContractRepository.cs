using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Abstractions.Persistence;
using ItilPaymentFlow.Domain.Presale;
using Microsoft.EntityFrameworkCore;

namespace ItilPaymentFlow.Infrastructure.Persistence.Repositories;

internal sealed class ContractRepository : IContractRepository
{
    private readonly PaymentDbContext _context;
    public ContractRepository(PaymentDbContext context) => _context = context;

    public async Task AddAsync(Contract contract, CancellationToken cancellationToken = default)
        => await _context.Set<Contract>().AddAsync(contract, cancellationToken);

    public Task<Contract?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => _context.Set<Contract>().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public async Task<List<Contract>> ListAsync(CancellationToken cancellationToken = default)
        => await _context.Set<Contract>().OrderByDescending(c => c.CreatedAtUtc).ToListAsync(cancellationToken);

    public IQueryable<Contract> Query() => _context.Set<Contract>().AsQueryable();
}