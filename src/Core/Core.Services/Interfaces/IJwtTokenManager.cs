using Core.Services.Authentication;
using System.Security.Claims;

namespace Core.Services.Interfaces;
public interface IJwtTokenManager
{
    Task<AuthResult> GenerateClaimsTokenAsync(string username, CancellationToken cancellationToken);
    Task<ClaimsPrincipal> GetPrincipFromTokenAsync(string token);
}
