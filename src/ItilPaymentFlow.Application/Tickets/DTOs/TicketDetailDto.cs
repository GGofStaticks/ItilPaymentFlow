using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItilPaymentFlow.Application.Tickets.DTOs
{
    public sealed record TicketDetailsDto(
        string Id,
        string Number,
        string Title,
        int Priority,
        string Description,
        string Contacts,
        string? Attachments,
        string Status,
        string CreatedAt // dd.MM.yyyy
    );
}