using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Assignments.DTOs;
using MediatR;

namespace ItilPaymentFlow.Application.Assignments.Commands
{
    public sealed record SubmitAssignmentCommand(Guid AssignmentId, Guid UserId, string? SubmissionText, string? File) : IRequest<CompletedAssignmentDto>;
}
