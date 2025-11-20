using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Domain.Abstractions;

namespace ItilPaymentFlow.Domain.Assignments
{
    public enum CompletedAssignmentStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3
    }

    public sealed class CompletedAssignment : AggregateRoot<Guid>
    {
        private CompletedAssignment() { }

        public CompletedAssignment(Guid id, Guid assignmentId, Guid userId, string? submissionText, string? fileUrl)
        {
            Id = id;
            AssignmentId = assignmentId;
            UserId = userId;
            SubmissionText = submissionText;
            FileUrl = fileUrl;
            Status = CompletedAssignmentStatus.Pending;
            CreatedAtUtc = DateTime.UtcNow;
        }

        public static CompletedAssignment Create(Guid assignmentId, Guid userId, string? submissionText, string? fileUrl)
            => new CompletedAssignment(Guid.NewGuid(), assignmentId, userId, submissionText, fileUrl);

        public Guid AssignmentId { get; private set; }
        public Guid UserId { get; private set; }
        public string? SubmissionText { get; private set; }
        public string? FileUrl { get; private set; }
        public CompletedAssignmentStatus Status { get; private set; }
        public int AwardedPoints { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }

        public void SetStatus(CompletedAssignmentStatus status, int awardedPoints = 0)
        {
            Status = status;
            AwardedPoints = awardedPoints;
        }

        public void SetFileUrl(string url) => FileUrl = url;
    }
}
