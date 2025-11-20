using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Abstractions.Persistence;
using ItilPaymentFlow.Application.Assignments.Commands;
using ItilPaymentFlow.Application.Assignments.DTOs;
using ItilPaymentFlow.Domain.Assignments;
using MediatR;

namespace ItilPaymentFlow.Application.Assignments.Handlers
{
    internal sealed class CreateAssignmentHandler : IRequestHandler<CreateAssignmentCommand, AssignmentDto>
    {
        private readonly IAssignmentRepository _repo;
        private readonly IUnitOfWork _uow;

        public CreateAssignmentHandler(IAssignmentRepository repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<AssignmentDto> Handle(CreateAssignmentCommand request, CancellationToken cancellationToken)
        {
            var entity = Assignment.Create(request.Title, request.ShortDescription, request.Points);
            await _repo.AddAsync(entity, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

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
