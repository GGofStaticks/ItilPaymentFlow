using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Abstractions;
using ItilPaymentFlow.Application.Abstractions.Persistence;
using ItilPaymentFlow.Application.Abstractions.Security;
using ItilPaymentFlow.Application.Users.Commands.Auth;
using MediatR;


namespace ItilPaymentFlow.Application.Users.Handlers
{
    internal sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginResultDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        private readonly IDateTimeProvider _clock;

        public RefreshTokenCommandHandler(IUserRepository userRepository, ISessionRepository sessionRepository, IUnitOfWork unitOfWork, IJwtService jwtService, IDateTimeProvider clock)
        {
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _clock = clock;
        }

        public async Task<LoginResultDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var session = await _sessionRepository.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken);
            if (session == null || session.RefreshTokenExpiresAt == null || session.RefreshTokenExpiresAt <= DateTime.UtcNow)
                throw new UnauthorizedAccessException("Invalid refresh token");

            // пользователь должен существовать — session.UserId гуид
            var user = await _userRepository.GetByIdAsync(session.UserId, cancellationToken)
                       ?? throw new UnauthorizedAccessException("User not found");

            var accessToken = _jwtService.GenerateAccessToken(user);
            var (newRefreshToken, expiresAt) = _jwtService.GenerateRefreshToken();

            // обновляем сессию новым рефреш токеном
            session.SetRefreshToken(newRefreshToken, expiresAt);
            await _sessionRepository.Update(session);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new LoginResultDto(accessToken, newRefreshToken, expiresAt);
        }
    }
}