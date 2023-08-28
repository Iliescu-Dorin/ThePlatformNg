using Authentication.Application.Interfaces;
using Authentication.Application.Oauth;
using Core.Services.Authentication.Interfaces;
using Core.Services.Authentication.Oauth;
using Core.Services.Authentication.Options;
using Core.SharedKernel.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System.Text;

namespace Core.Services.Authentication.Service.Extensions;

public static class ConfigureJWTBearer
{
    public static void AddJWTBearerConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        // Register OAuth services
        services.AddTransient<IJwtTokenManager, JwtTokenManager>();
        services.AddScoped<ISignInManager, SignInManager>();

        // Configure JWT Authentication and Authorization
        var jwtSettings = new JwtSettings();
        configuration.Bind(nameof(JwtSettings), jwtSettings);
        services.AddSingleton(jwtSettings);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
            ValidateIssuer = jwtSettings.ValidateIssuer,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = jwtSettings.ValidateAudience,
            ValidAudience = jwtSettings.Audience,
            RequireExpirationTime = jwtSettings.RequireExpirationTime,
            ValidateLifetime = jwtSettings.ValidateLifetime,
            ClockSkew = jwtSettings.Expiration
        };
        services.AddSingleton(tokenValidationParameters);

        services.ConfigureOptions<ConfigureJwtBearerOptions>()
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = tokenValidationParameters;
            });

        // Method 1
        services.Configure<JWTTokenConfig>(configuration.GetSection("JwtTokenConfig"))
            .AddSingleton(x => x.GetRequiredService<IOptions<JWTTokenConfig>>().Value);

        // Method 2
        // configure strongly typed settings object
        var AppSettings = new AppSettings();
        configuration.GetSection("AppSettings").Bind(AppSettings);
        // Now start using it
        string OauthIssuer = AppSettings.OAuthIssuer;
        string OauthClientId = AppSettings.OAuthClientId;
        var OauthSecret = TextEncodings.Base64Url.Decode(AppSettings.OAuthSecret);
        services.AddAuthentication(auth =>
        {
            auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddCookie(cookie =>
        {
            cookie.AccessDeniedPath = "logout";
            cookie.SlidingExpiration = true;
        })
        .AddJwtBearer(jwt =>
        {
            jwt.Audience = OauthClientId;
            jwt.Authority = OauthIssuer;
            jwt.RequireHttpsMetadata = false;
            jwt.SaveToken = true;
            jwt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = OauthIssuer,
                ValidAudience = OauthClientId,
                IssuerSigningKey = new SymmetricSecurityKey(OauthSecret),
                ValidateIssuer = true,
                RequireAudience = true,
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false,
                ValidateTokenReplay = false,
                ValidateActor = false,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero
            };
            jwt.Configuration = new OpenIdConnectConfiguration();
        });
    }
}
