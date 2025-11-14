using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Domain.Users;

namespace ItilPaymentFlow.Application.Abstractions.Security
{
    public interface IJwtService
    {
        string GenerateAccessToken(User user);
        (string refreshToken, DateTime expiresAt) GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}