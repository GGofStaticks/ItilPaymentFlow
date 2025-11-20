using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItilPaymentFlow.Application.Assignments.DTOs
{
    public sealed class CompletedAssignmentDto
    {
        public Guid Id { get; set; }
        public Guid AssignmentId { get; set; }
        public Guid UserId { get; set; }
        public string? SubmissionText { get; set; }
        public string? FileUrl { get; set; }
        public string Status { get; set; } = string.Empty;
        public int AwardedPoints { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}
