using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Abstractions.Persistence;
using ItilPaymentFlow.Application.Assignments.DTOs;
using ItilPaymentFlow.Application.Assignments.Queries;
using MediatR;

namespace ItilPaymentFlow.Application.Assignments.Handlers
{
    internal sealed class GetAssignmentHandler : IRequestHandler<GetAssignmentQuery, AssignmentDto?>
    {
        private readonly IAssignmentRepository _repo;

        public GetAssignmentHandler(IAssignmentRepository repo) =>
            _repo = repo;

        public async Task<AssignmentDto?> Handle(GetAssignmentQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null) return null;

            return new AssignmentDto
            {
                Id = entity.Id,
                Title = entity.Title,
                ShortDescription = entity.ShortDescription,
                Points = entity.Points,
                Active = entity.Active,
                CreatedAtUtc = entity.CreatedAtUtc
            };
        }
    }
}
