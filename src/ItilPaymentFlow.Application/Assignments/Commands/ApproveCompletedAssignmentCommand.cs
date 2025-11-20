using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ItilPaymentFlow.Application.Assignments.Commands
{
    public sealed record ApproveCompletedAssignmentCommand(
        Guid CompletedAssignmentId,
        int AwardedPoints,
        bool Approve
    ) : IRequest<Unit>;
}
