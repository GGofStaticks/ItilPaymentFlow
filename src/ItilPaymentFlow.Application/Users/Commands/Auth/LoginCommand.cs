using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ItilPaymentFlow.Application.Users.Commands.Auth
{
    public record LoginCommand(string Email, string Password) : IRequest<LoginResultDto>;
}