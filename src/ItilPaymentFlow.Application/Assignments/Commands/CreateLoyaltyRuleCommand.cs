using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Assignments.DTOs;
using MediatR;

namespace ItilPaymentFlow.Application.Assignments.Commands
{
    public sealed record CreateLoyaltyRuleCommand(
        string Name,
        int PointsForAssignmentType,
        int LevelThreshold
    ) : IRequest<LoyaltyRuleDto>;
}
