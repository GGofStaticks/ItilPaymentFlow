using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItilPaymentFlow.Application.Assignments.DTOs
{
    public sealed class AssignmentDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public int Points { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}
