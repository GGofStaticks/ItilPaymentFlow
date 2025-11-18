using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Tickets.DTOs;
using MediatR;

namespace ItilPaymentFlow.Application.Tickets.Queries.ListTickets
{
    public sealed class ListTicketsQuery : IRequest<PagedResult<TicketDto>>
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 5;

        public ListTicketsQuery() { }
    }
}