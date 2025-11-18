using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItilPaymentFlow.Application.Presale.DTOs
{
    public sealed class ContractDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string? FileUrl { get; set; }
        public Guid CounterpartyId { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}