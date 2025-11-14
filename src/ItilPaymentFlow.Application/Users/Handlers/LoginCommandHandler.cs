using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Abstractions;
using ItilPaymentFlow.Application.Abstractions.Persistence;
using ItilPaymentFlow.Application.Abstractions.Security;
using ItilPaymentFlow.Application.Security;
using ItilPaymentFlow.Application.Users.Commands.Auth;
using ItilPaymentFlow.Domain.Sessions;
using ItilPaymentFlow.Domain.ValueObjects;
using MediatR;

namespace ItilPaymentFlow.Application.Users.Handlers
{
    internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResultDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        private readonly IDateTimeProvider _clock;

        public LoginCommandHandler(IUserRepository userRepository, ISessionRepository sessionRepository, IUnitOfWork unitOfWork, IJwtService jwtService, IDateTimeProvider clock)
        {
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _clock = clock;
        }

        public async Task<LoginResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken)
                       ?? throw new UnauthorizedAccessException("Invalid login details");

            if (!PasswordHasher.Verify(user.PasswordHash, request.Password))
                throw new UnauthorizedAccessException("Invalid login details");

            var accessToken = _jwtService.GenerateAccessToken(user);
            var (refreshToken, expiresAt) = _jwtService.GenerateRefreshToken();

            var started = DateTime.SpecifyKind(_clock.UtcNow, DateTimeKind.Utc);

            // здес User.Id это гуид, поэтому передача гуид
            var session = Session.Create(
                user.Id,           // гуид
                refreshToken,
                expiresAt,
                started
            );
            await _sessionRepository.Add(session);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new LoginResultDto(accessToken, refreshToken, expiresAt);
        }
    }
}