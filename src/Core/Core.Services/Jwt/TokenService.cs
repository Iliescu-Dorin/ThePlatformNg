using Core.Services.Interfaces;
using Core.SharedKernel.Config;
using Core.SharedKernel.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Core.Services.Jwt;
public class TokenService : ITokenService
{
    private readonly JWTTokenConfig _config;

    public TokenService(JWTTokenConfig tokenConfig)
    {
        _config = tokenConfig;
    }

    public AuthTokenDTO Generate(UserDTO user)
    {
        List<Claim> claims = new() {
                new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim (JwtRegisteredClaimNames.Email, user.EmailAddress),
                new Claim (JwtRegisteredClaimNames.Sub, user.EmailAddress),
                new Claim (ClaimTypes.Role, user.Role.ToString())
            };

        JwtSecurityToken token = new TokenBuilder()
        .AddAudience(_config.Audience)
        .AddIssuer(_config.Issuer)
        .AddExpiry(_config.ExpiryInMinutes)
        .AddKey(_config.key)
        .AddClaims(claims)
        .Build();

        string accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        return new AuthTokenDTO
        {
            AccessToken = accessToken,
            ExpiresIn = _config.ExpiryInMinutes
        };
    }
}
