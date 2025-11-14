using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Abstractions.Persistence;
using ItilPaymentFlow.Application.Tickets.DTOs;
using MediatR;

namespace ItilPaymentFlow.Application.Tickets.Queries.GetTicket
{
    internal sealed class GetTicketQueryHandler : IRequestHandler<GetTicketQuery, TicketDto?>
    {
        private readonly ITicketRepository _repository;
        public GetTicketQueryHandler(ITicketRepository repository) => _repository = repository;

        public async Task<TicketDto?> Handle(GetTicketQuery request, CancellationToken cancellationToken)
        {
            // попытка парса гуид, если неправильный, возврат нул
            if (!Guid.TryParse(request.Id, out var guid)) return null;

            var t = await _repository.GetByIdAsync(guid, cancellationToken);
            if (t is null) return null;

            return new TicketDto
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
            };
        }
    }
}