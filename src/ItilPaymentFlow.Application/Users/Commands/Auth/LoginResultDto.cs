using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItilPaymentFlow.Application.Users.Commands.Auth
{
    public record LoginResultDto(string AccessToken, string RefreshToken, DateTime RefreshTokenExpiresAt);
}