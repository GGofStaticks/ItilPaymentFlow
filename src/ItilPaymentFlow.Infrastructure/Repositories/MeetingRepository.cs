using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Abstractions.Persistence;
using ItilPaymentFlow.Domain.Presale;
using Microsoft.EntityFrameworkCore;

namespace ItilPaymentFlow.Infrastructure.Persistence.Repositories;

internal sealed class MeetingRepository : IMeetingRepository
{
    private readonly PaymentDbContext _context;
    public MeetingRepository(PaymentDbContext context) => _context = context;

    public async Task AddAsync(Meeting meeting, CancellationToken cancellationToken = default)
        => await _context.Set<Meeting>().AddAsync(meeting, cancellationToken);

    public Task<Meeting?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => _context.Set<Meeting>().FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

    public async Task<List<Meeting>> ListAsync(CancellationToken cancellationToken = default)
        => await _context.Set<Meeting>().OrderByDescending(m => m.At).ToListAsync(cancellationToken);

    public IQueryable<Meeting> Query() => _context.Set<Meeting>().AsQueryable();
}