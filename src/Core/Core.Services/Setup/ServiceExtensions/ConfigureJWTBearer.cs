using Core.SharedKernel.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security.DataHandler.Encoder;

namespace Core.Services.Setup.ServiceExtensions;

public static class ConfigureJWTBearer
{
    public static void AddJWTBearerConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
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
