using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Domain.Tickets;


namespace ItilPaymentFlow.Application.Abstractions.Persistence
{
    public interface ITicketRepository
    {
        Task AddAsync(Ticket ticket, CancellationToken cancellationToken = default);
        Task<Ticket?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Ticket>> ListAsync(CancellationToken cancellationToken = default);
        IQueryable<Ticket> Query();
    }
}