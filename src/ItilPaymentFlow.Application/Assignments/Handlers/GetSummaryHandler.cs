using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Abstractions.Persistence;
using ItilPaymentFlow.Application.Assignments.DTOs;
using ItilPaymentFlow.Application.Assignments.Queries;
using ItilPaymentFlow.Domain.Assignments;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItilPaymentFlow.Application.Assignments.Handlers
{
    internal sealed class GetSummaryHandler : IRequestHandler<GetSummaryQuery, SummaryDto>
    {
        private readonly ICompletedAssignmentRepository _completedRepo;
        private readonly ILoyaltyRuleRepository _rulesRepo;

        public GetSummaryHandler(ICompletedAssignmentRepository completedRepo, ILoyaltyRuleRepository rulesRepo)
        {
            _completedRepo = completedRepo;
            _rulesRepo = rulesRepo;
        }

        public async Task<SummaryDto> Handle(GetSummaryQuery request, CancellationToken cancellationToken)
        {
            var completed = _completedRepo.Query().Where(c => c.UserId == request.UserId && c.Status == CompletedAssignmentStatus.Approved);

            var total = await completed.SumAsync(c => (int?)c.AwardedPoints, cancellationToken) ?? 0;
            var monthFrom = DateTime.UtcNow.AddMonths(-1);
            var monthPoints = await completed.Where(c => c.CreatedAtUtc >= monthFrom).SumAsync(c => (int?)c.AwardedPoints, cancellationToken) ?? 0;

            var rules = await _rulesRepo.ListAsync(cancellationToken);
            var nextRule = rules.FirstOrDefault(r => r.LevelThreshold > total);
            var toNext = nextRule == null ? 0 : Math.Max(0, nextRule.LevelThreshold - total);

            return new SummaryDto
            {
                TotalPoints = total,
                PointsThisMonth = monthPoints,
                PointsToNextLevel = toNext
            };
        }
    }
}
