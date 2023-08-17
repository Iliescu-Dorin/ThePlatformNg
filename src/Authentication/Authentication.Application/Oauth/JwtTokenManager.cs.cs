using Authentication.Application.Interfaces;
using Authentication.Domain;
using Core.Services.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication.Application.Oauth
{
    public class JwtTokenManager : IJwtTokenManager
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public JwtTokenManager(
            JwtSettings jwtSettings,
            UserManager<ApplicationUser> userManager,
            TokenValidationParameters tokenValidationParameters)
        {
            _jwtSettings = jwtSettings;
            _userManager = userManager;
            _tokenValidationParameters = tokenValidationParameters;
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public async Task<AuthResult> GenerateClaimsTokenAsync(string username, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(username);

            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaimsIdentity(user.Id, username),
                Expires = DateTime.UtcNow.Add(_jwtSettings.Expiration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var refreshTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateRefreshClaimsIdentity(user.Id, username),
                Expires = DateTime.UtcNow.Add(_jwtSettings.Expiration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = _tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = _tokenHandler.CreateToken(refreshTokenDescriptor);

            return new AuthResult
            {
                AccessToken = _tokenHandler.WriteToken(token),
                TokenType = "Bearer",
                ExpiresIn = (int)_jwtSettings.Expiration.TotalSeconds,
                RefreshToken = _tokenHandler.WriteToken(refreshToken)
            };
        }

        public async Task<ClaimsPrincipal> GetPrincipFromTokenAsync(string token)
        {
            try
            {
                var tokenValidationParams = _tokenValidationParameters.Clone();
                tokenValidationParams.ValidateLifetime = false;

                var principal = _tokenHandler.ValidateToken(token, tokenValidationParams, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }

                return await Task.FromResult(principal);
            }
            catch
            {
                return null;
            }
        }

        private ClaimsIdentity GenerateClaimsIdentity(string userId, string username)
        {
            return new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Nbf, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddMinutes(5).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            });
        }

        private ClaimsIdentity GenerateRefreshClaimsIdentity(string userId, string username)
        {
            return new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Iss, _jwtSettings.Issuer),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Nbf, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddMinutes(30).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            });
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return validatedToken is JwtSecurityToken jwtSecurityToken &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
