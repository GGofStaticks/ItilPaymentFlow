using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItilPaymentFlow.Application.Presale.DTOs
{
    public sealed class OfferDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Number { get; set; } = string.Empty;
        public DateTime ValidUntil { get; set; }
        public string? FileUrl { get; set; }
        public Guid SupplierId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAtUtc { get; set; }
    }
}