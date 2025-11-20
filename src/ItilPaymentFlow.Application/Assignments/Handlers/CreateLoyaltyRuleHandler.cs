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
    public sealed class CreateLoyaltyRuleHandler
        : IRequestHandler<CreateLoyaltyRuleCommand, LoyaltyRuleDto>
    {
        private readonly ILoyaltyRuleRepository _repo;
        private readonly IUnitOfWork _uow;

        public CreateLoyaltyRuleHandler(ILoyaltyRuleRepository repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<LoyaltyRuleDto> Handle(
            CreateLoyaltyRuleCommand request,
            CancellationToken cancellationToken)
        {
            var entity = LoyaltyRule.Create(
                request.Name,
                request.PointsForAssignmentType,
                request.LevelThreshold
            );

            await _repo.AddAsync(entity, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return new LoyaltyRuleDto
            {
                Id = entity.Id,
                Name = entity.Name,
                PointsForAssignmentType = entity.PointsForAssignmentType,
                LevelThreshold = entity.LevelThreshold,
                CreatedAtUtc = entity.CreatedAtUtc
            };
        }
    }
}