using Authentication.Domain;
using Core.Services.Authentication;
using Core.Services.DataEncryption;
using Core.Services.Interfaces;
using Core.Services.Jwt.Oauth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Authentication.Infrastructure.Services;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastrucutre(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
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

        services.AddAuthentication(options =>
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

        // Register Identity DbContext and Server
        services.AddDbContext<AuthDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Net6WebApiConnection")));

        var identityOptionsConfig = new IdentityOptions();
        configuration.GetSection(nameof(IdentityOptions)).Bind(identityOptionsConfig);

        services.AddDefaultIdentity<ApplicationUser>(options =>
        {
            options.Password.RequiredLength = identityOptionsConfig.RequiredLength;
            options.Password.RequireDigit = identityOptionsConfig.RequiredDigit;
            options.Password.RequireLowercase = identityOptionsConfig.RequireLowercase;
            options.Password.RequiredUniqueChars = identityOptionsConfig.RequiredUniqueChars;
            options.Password.RequireUppercase = identityOptionsConfig.RequireUppercase;
            options.Lockout.MaxFailedAccessAttempts = identityOptionsConfig.MaxFailedAttempts;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(identityOptionsConfig.LockoutTimeSpanInDays);
        })
        .AddEntityFrameworkStores<AuthDbContext>();

        // Register Data Protection Services
        services.AddDataProtection()
            .SetDefaultKeyLifetime(TimeSpan.FromDays(30));
        services.AddSingleton<IDataEncryption, RouteDataProtection>();

        return services;
    }
}
