using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ItilPaymentFlow.Application.Users.Commands.UpdateUser
{
    public sealed record UpdateUserCommand(Guid Id, string? Email, string? Password, string? Role) : IRequest<Unit>;

}
