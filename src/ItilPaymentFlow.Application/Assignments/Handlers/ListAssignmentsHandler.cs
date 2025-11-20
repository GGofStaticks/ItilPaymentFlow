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
    public sealed class ListAssignmentsHandler
        : IRequestHandler<ListAssignmentsQuery, PagedResult<AssignmentDto>>
    {
        private readonly IAssignmentRepository _repo;

        public ListAssignmentsHandler(IAssignmentRepository repo)
        {
            _repo = repo;
        }

        public async Task<PagedResult<AssignmentDto>> Handle(
            ListAssignmentsQuery request,
            CancellationToken cancellationToken)
        {
            var query = _repo.Query();

            // всего записей
            var totalCount = await query.CountAsync(cancellationToken);

            // получаем страницу
            var items = await query
                .OrderByDescending(x => x.CreatedAtUtc)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new AssignmentDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    ShortDescription = x.ShortDescription,
                    Points = x.Points,
                    Active = x.Active,
                    CreatedAtUtc = x.CreatedAtUtc
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<AssignmentDto>(
                items,
                request.Page,
                request.PageSize,
                totalCount);
        }
    }
}