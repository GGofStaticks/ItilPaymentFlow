using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Assignments.DTOs;
using ItilPaymentFlow.Application.Tickets.Queries.ListTickets;
using MediatR;

namespace ItilPaymentFlow.Application.Assignments.Queries
{
    public sealed record ListLoyaltyRulesQuery(int Page = 1, int PageSize = 5)
        : IRequest<PagedResult<LoyaltyRuleDto>>;
}