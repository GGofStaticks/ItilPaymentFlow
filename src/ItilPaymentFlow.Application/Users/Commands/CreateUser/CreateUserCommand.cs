using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ItilPaymentFlow.Application.Users.Commands.CreateUser
{
    public record CreateUserCommand(
        string Email,
        string Password,
        string? Role,
        string FirstName,
        string LastName,
        string? MiddleName
    ) : IRequest<Guid>;
}