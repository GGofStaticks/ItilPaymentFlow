using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Tickets.DTOs;
using MediatR;

namespace ItilPaymentFlow.Application.Tickets.Commands.CreateTicket
{
    public sealed record CreateTicketCommand(string Title, int Priority, string Description, string Contacts, List<string>? Attachments, Guid AuthorId) : IRequest<TicketDto>;
}