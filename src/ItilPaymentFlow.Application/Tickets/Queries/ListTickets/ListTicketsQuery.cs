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
        public int Page { get; }
        public int PageSize { get; }

        public ListTicketsQuery(int page = 1, int pageSize = 5)
        {
            Page = page < 1 ? 1 : page;
            PageSize = pageSize <= 0 ? 5 : pageSize;
        }
    }
}