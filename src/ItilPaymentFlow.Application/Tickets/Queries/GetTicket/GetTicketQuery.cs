using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Tickets.DTOs;
using MediatR;

namespace ItilPaymentFlow.Application.Tickets.Queries.GetTicket
{
    // Прием string, тк контроллер отдаёт string id (маршрутный параметр)
    public sealed record GetTicketQuery(string Id) : IRequest<TicketDto?>;
}