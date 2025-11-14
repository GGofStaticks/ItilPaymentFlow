using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Abstractions.Persistence;
using ItilPaymentFlow.Application.Users.Commands.Auth;
using ItilPaymentFlow.Domain.ValueObjects;
using MediatR;

namespace ItilPaymentFlow.Application.Users.Handlers
{
    internal sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly ISessionRepository _session_repository;
        private readonly IUnitOfWork _unitOfWork;

        public LogoutCommandHandler(
            IUserRepository userRepository,
            ISessionRepository sessionRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _session_repository = sessionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
                       ?? throw new KeyNotFoundException("User not found");

            var sessions = await _session_repository.GetAllActiveByUserIdAsync(
                user.Id, // user.Id - Guid
                cancellationToken
            );

            foreach (var session in sessions)
            {
                session.EndSession();
                await _session_repository.Update(session);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}