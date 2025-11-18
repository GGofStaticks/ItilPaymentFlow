using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Abstractions.Persistence;
using ItilPaymentFlow.Domain.Presale;
using Microsoft.EntityFrameworkCore;

namespace ItilPaymentFlow.Infrastructure.Persistence.Repositories;

internal sealed class OfferRepository : IOfferRepository
{
    private readonly PaymentDbContext _context;
    public OfferRepository(PaymentDbContext context) => _context = context;

    public async Task AddAsync(Offer offer, CancellationToken cancellationToken = default)
        => await _context.Set<Offer>().AddAsync(offer, cancellationToken);

    public async Task UpdateAsync(Offer offer, CancellationToken cancellationToken = default)
    {
        _context.Set<Offer>().Update(offer);
        await Task.CompletedTask;
    }

    public Task<Offer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => _context.Set<Offer>().FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

    public async Task<List<Offer>> ListAsync(CancellationToken cancellationToken = default)
        => await _context.Set<Offer>().OrderByDescending(o => o.CreatedAtUtc).ToListAsync(cancellationToken);

    public IQueryable<Offer> Query() => _context.Set<Offer>().AsQueryable();
}