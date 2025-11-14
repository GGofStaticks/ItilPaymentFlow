using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Abstractions.Persistence;
using ItilPaymentFlow.Application.Tickets.DTOs;
using MediatR;

namespace ItilPaymentFlow.Application.Tickets.Queries.ListTickets
{
    internal sealed class ListTicketsQueryHandler : IRequestHandler<ListTicketsQuery, List<TicketDto>>
    {
        private readonly ITicketRepository _repository;

        public ListTicketsQueryHandler(ITicketRepository repository) => _repository = repository;

        public async Task<List<TicketDto>> Handle(ListTicketsQuery request, CancellationToken cancellationToken)
        {
            var tickets = await _repository.ListAsync(cancellationToken);

            return tickets.Select(t => new TicketDto
            {
                Id = t.Id,
                Number = t.Number,
                Title = t.Title,
                Priority = t.Priority,
                Description = t.Description,
                Contacts = t.Contacts,
                Attachments = t.Attachments,
                CreatedAtUtc = t.CreatedAtUtc,
                SlaTime = t.SlaTime,
                Status = t.Status.ToString()
            }).ToList();
        }
    }
}