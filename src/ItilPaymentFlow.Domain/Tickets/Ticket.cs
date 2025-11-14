using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Domain.Abstractions;
using ItilPaymentFlow.Domain.Users;
using ItilPaymentFlow.Domain.ValueObjects;

namespace ItilPaymentFlow.Domain.Tickets
{
    public enum TicketStatus
    {
        InProgress,
        Paused,
        Cancelled,
        Done
    }

    public sealed class Ticket
    {
        public Guid Id { get; private set; }

        public string Number { get; private set; } = string.Empty;
        public string Title { get; private set; } = string.Empty;
        public int Priority { get; private set; }
        public string Description { get; private set; } = string.Empty;
        public string Contacts { get; private set; } = string.Empty;
        public string? Attachments { get; private set; }

        public DateTime CreatedAtUtc { get; private set; }
        public DateTime? SlaTime { get; private set; }

        public TicketStatus Status { get; private set; }

        public Guid AuthorId { get; private set; }
        public User Author { get; private set; } = default!;

        private Ticket() { }

        private Ticket(Guid id, string number, string title, int priority, string description, string contacts, string? attachments, DateTime createdAtUtc, DateTime? slaTime)
        {
            Id = id;
            Number = number;
            Title = title;
            Priority = priority;
            Description = description;
            Contacts = contacts;
            Attachments = attachments;
            CreatedAtUtc = createdAtUtc;
            SlaTime = slaTime;
            Status = TicketStatus.InProgress;
        }

        public static Ticket Create(
            string title,
            int priority,
            string description,
            string contacts,
            string? attachments,
            DateTime createdAtUtc,
            string number,
            Guid authorId)
        {
            var sla = createdAtUtc.AddDays(3);

            return new Ticket(
                Guid.NewGuid(),
                number,
                title.Trim(),
                priority,
                description?.Trim() ?? string.Empty,
                contacts?.Trim() ?? string.Empty,
                attachments,
                createdAtUtc,
                sla)
            {
                AuthorId = authorId
            };
        }

        public void SetStatus(TicketStatus status) => Status = status;
    }
}