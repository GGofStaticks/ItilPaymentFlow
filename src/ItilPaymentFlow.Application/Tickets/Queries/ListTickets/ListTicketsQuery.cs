using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Tickets.DTOs;
using MediatR;

namespace ItilPaymentFlow.Application.Tickets.Queries.ListTickets
{
    public sealed class ListTicketsQuery : IRequest<List<TicketDto>> { }
}