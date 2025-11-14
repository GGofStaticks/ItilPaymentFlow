using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Abstractions.Persistence;
using ItilPaymentFlow.Application.Security;
using ItilPaymentFlow.Application.Users.Commands.UpdateUser;
using MediatR;

namespace ItilPaymentFlow.Application.Users.Handlers
{
    internal sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken)
                       ?? throw new KeyNotFoundException("User not found");

            if (!string.IsNullOrWhiteSpace(request.Email))
                user.UpdateEmail(request.Email);

            if (!string.IsNullOrWhiteSpace(request.Password))
                user.UpdatePasswordHash(PasswordHasher.Hash(request.Password));

            if (request.Role is not null)
                user.SetRole(request.Role);

            await _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}