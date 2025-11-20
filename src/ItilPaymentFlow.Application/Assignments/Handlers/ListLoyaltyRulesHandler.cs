using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Abstractions.Persistence;
using ItilPaymentFlow.Application.Assignments.DTOs;
using ItilPaymentFlow.Application.Assignments.Queries;
using ItilPaymentFlow.Application.Tickets.Queries.ListTickets;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItilPaymentFlow.Application.Assignments.Handlers
{
    public sealed class ListLoyaltyRulesHandler
        : IRequestHandler<ListLoyaltyRulesQuery, PagedResult<LoyaltyRuleDto>>
    {
        private readonly ILoyaltyRuleRepository _repo;

        public ListLoyaltyRulesHandler(ILoyaltyRuleRepository repo)
        {
            _repo = repo;
        }

        public async Task<PagedResult<LoyaltyRuleDto>> Handle(
            ListLoyaltyRulesQuery request,
            CancellationToken cancellationToken)
        {
            var query = _repo.Query();

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(x => x.LevelThreshold)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new LoyaltyRuleDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    PointsForAssignmentType = x.PointsForAssignmentType,
                    LevelThreshold = x.LevelThreshold,
                    CreatedAtUtc = x.CreatedAtUtc
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<LoyaltyRuleDto>(
                items,
                request.Page,
                request.PageSize,
                totalCount
            );
        }
    }
}