using Core.SharedKernel.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Core.Services.Options;

public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly JWTTokenConfig _configuration;

    public ConfigureJwtBearerOptions(JWTTokenConfig configuration)
    {
        _configuration = configuration;
    }

    public void Configure(string name, JwtBearerOptions options)
    {
        if (name == JwtBearerDefaults.AuthenticationScheme)
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.key)),
                ValidIssuer = _configuration.Issuer,
                ValidAudience = _configuration.Audience
            };

            options.Events = new JwtBearerEvents()
            {
                OnAuthenticationFailed = (context) =>
                {
                    Console.WriteLine(context.Exception);
                    return Task.CompletedTask;
                },

                OnMessageReceived = (context) =>
                {
                    return Task.CompletedTask;
                },

                OnTokenValidated = (context) =>
                {
                    return Task.CompletedTask;
                }
            };
        }
    }

    public void Configure(JwtBearerOptions options)
    {
        Configure(JwtBearerDefaults.AuthenticationScheme, options);
    }
}
