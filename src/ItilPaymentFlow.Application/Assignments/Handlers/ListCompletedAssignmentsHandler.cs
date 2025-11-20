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
    public sealed class ListCompletedAssignmentsHandler
        : IRequestHandler<ListCompletedAssignmentsQuery, PagedResult<CompletedAssignmentDto>>
    {
        private readonly ICompletedAssignmentRepository _completedRepo;

        public ListCompletedAssignmentsHandler(ICompletedAssignmentRepository completedRepo)
        {
            _completedRepo = completedRepo;
        }

        public async Task<PagedResult<CompletedAssignmentDto>> Handle(
            ListCompletedAssignmentsQuery request,
            CancellationToken cancellationToken)
        {
            var query = _completedRepo.Query()
                .OrderByDescending(c => c.CreatedAtUtc);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(c => new CompletedAssignmentDto
                {
                    Id = c.Id,
                    AssignmentId = c.AssignmentId,
                    UserId = c.UserId,
                    SubmissionText = c.SubmissionText,
                    FileUrl = c.FileUrl,
                    Status = c.Status.ToString(),
                    AwardedPoints = c.AwardedPoints,
                    CreatedAtUtc = c.CreatedAtUtc
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<CompletedAssignmentDto>(
                items,
                request.Page,
                request.PageSize,
                totalCount
            );
        }
    }
}