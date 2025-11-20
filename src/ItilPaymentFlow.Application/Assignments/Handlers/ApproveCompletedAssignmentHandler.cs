using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Abstractions.Persistence;
using ItilPaymentFlow.Application.Assignments.Commands;
using ItilPaymentFlow.Domain.Assignments;
using MediatR;

namespace ItilPaymentFlow.Application.Assignments.Handlers
{
    internal sealed class ApproveCompletedAssignmentHandler : IRequestHandler<ApproveCompletedAssignmentCommand, Unit>
    {
        private readonly ICompletedAssignmentRepository _repo;
        private readonly IAssignmentRepository _assignmentRepo;
        private readonly IUnitOfWork _uow;

        public ApproveCompletedAssignmentHandler(ICompletedAssignmentRepository repo, IAssignmentRepository assignmentRepo, IUnitOfWork uow)
        {
            _repo = repo;
            _assignmentRepo = assignmentRepo;
            _uow = uow;
        }

        public async Task<Unit> Handle(ApproveCompletedAssignmentCommand request, CancellationToken cancellationToken)
        {
            var completed = await _repo.GetByIdAsync(request.CompletedAssignmentId, cancellationToken)
                            ?? throw new KeyNotFoundException("Completed assignment not found");

            if (request.Approve)
            {
                completed.SetStatus(CompletedAssignmentStatus.Approved, request.AwardedPoints);
            }
            else
            {
                completed.SetStatus(CompletedAssignmentStatus.Rejected, 0);
            }

            await _repo.UpdateAsync(completed, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
