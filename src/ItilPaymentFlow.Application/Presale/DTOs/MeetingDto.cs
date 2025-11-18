using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItilPaymentFlow.Application.Presale.DTOs
{
    public sealed class MeetingDto
    {
        public Guid Id { get; set; }
        public DateTime At { get; set; }
        public string Topic { get; set; } = string.Empty;
        public string Participants { get; set; } = string.Empty;
        public string? FileUrl { get; set; }
        public string? Link { get; set; }
        public Guid OrganiserId { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}