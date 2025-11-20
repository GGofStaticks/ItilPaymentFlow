using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Assignments.DTOs;
using MediatR;

namespace ItilPaymentFlow.Application.Assignments.Queries
{
    public sealed record GetSummaryQuery(Guid UserId) : IRequest<SummaryDto>;
}
