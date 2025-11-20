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
    internal sealed class SubmitAssignmentHandler : IRequestHandler<SubmitAssignmentCommand, CompletedAssignmentDto>
    {
        private readonly ICompletedAssignmentRepository _repo;
        private readonly IUnitOfWork _uow;

        public SubmitAssignmentHandler(ICompletedAssignmentRepository repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<CompletedAssignmentDto> Handle(SubmitAssignmentCommand request, CancellationToken cancellationToken)
        {
            var entity = CompletedAssignment.Create(request.AssignmentId, request.UserId, request.SubmissionText, request.File);
            await _repo.AddAsync(entity, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return new CompletedAssignmentDto
            {
                Id = entity.Id,
                AssignmentId = entity.AssignmentId,
                UserId = entity.UserId,
                SubmissionText = entity.SubmissionText,
                FileUrl = entity.FileUrl,
                Status = entity.Status.ToString(),
                AwardedPoints = entity.AwardedPoints,
                CreatedAtUtc = entity.CreatedAtUtc
            };
        }
    }
}
