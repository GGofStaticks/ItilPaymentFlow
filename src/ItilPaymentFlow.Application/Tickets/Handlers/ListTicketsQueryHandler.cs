using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Abstractions.Persistence;
using ItilPaymentFlow.Application.Tickets.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItilPaymentFlow.Application.Tickets.Queries.ListTickets
{
    internal sealed class ListTicketsQueryHandler
        : IRequestHandler<ListTicketsQuery, PagedResult<TicketDto>>
    {
        private readonly ITicketRepository _ticketRepository;

        public ListTicketsQueryHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<PagedResult<TicketDto>> Handle(ListTicketsQuery request, CancellationToken cancellationToken)
        {
            var query = _ticketRepository.Query()
                .Include(t => t.Author)            // если в домене есть Author
                .AsNoTracking();

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(t => t.CreatedAtUtc)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(t => new TicketDto
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
                    Status = t.Status.ToString(),

                    Author = t.Author == null
                        ? null
                        : new AuthorDto
                        {
                            Id = t.Author.Id,
                            FirstName = t.Author.FirstName,
                            LastName = t.Author.LastName,
                            Email = t.Author.Email
                        }
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<TicketDto>(
                items,
                request.Page,
                request.PageSize,
                totalCount
            );
        }
    }
}