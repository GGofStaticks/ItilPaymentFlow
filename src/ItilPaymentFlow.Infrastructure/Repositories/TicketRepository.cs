using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Abstractions.Persistence;
using ItilPaymentFlow.Domain.Tickets;
using ItilPaymentFlow.Domain.ValueObjects;
using ItilPaymentFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ItilPaymentFlow.Infrastructure.Persistence.Repositories
{
    internal sealed class TicketRepository : ITicketRepository
    {
        private readonly PaymentDbContext _context;

        public TicketRepository(PaymentDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Ticket ticket, CancellationToken cancellationToken = default)
        {
            await _context.Set<Ticket>().AddAsync(ticket, cancellationToken);
        }

        public async Task<Ticket?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Ticket>().FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<List<Ticket>> ListAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<Ticket>().ToListAsync(cancellationToken);
        }

        public IQueryable<Ticket> Query()
        {
            return _context.Tickets.AsQueryable();
        }
    }
}