using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Abstractions.Persistence;
using ItilPaymentFlow.Application.Security;
using ItilPaymentFlow.Application.Users.Commands.CreateUser;
using ItilPaymentFlow.Domain.Users;
using MediatR;

namespace ItilPaymentFlow.Application.Users.Handlers
{
    internal sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existing = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (existing != null)
                throw new InvalidOperationException("User with such email already exists.");

            var passwordHash = PasswordHasher.Hash(request.Password);
            var user = User.Create(request.Email, passwordHash, request.Role, request.FirstName, request.LastName, request.MiddleName);

            await _userRepository.Add(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return user.Id;
        }
    }
}