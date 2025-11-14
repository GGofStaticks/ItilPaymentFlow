using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItilPaymentFlow.Application.Tickets.DTOs
{
    public class TicketDto
    {
        public Guid Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int Priority { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Contacts { get; set; } = string.Empty;
        public string? Attachments { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? SlaTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public AuthorDto? Author { get; set; }
    }

    public sealed class AuthorDto
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
    }
}