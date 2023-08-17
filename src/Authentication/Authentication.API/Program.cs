using Authentication.Application.Interfaces;
using Authentication.Application.Oauth;
using Authentication.Domain;
using Authentication.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Authentication.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.builder.Services.AddAuthorization();

            // Register OAuth services
            builder.builder.Services.AddTransient<IJwtTokenManager, JwtTokenManager>();
            builder.builder.Services.AddScoped<ISignInManager, Application.SignInManager>();

            // Configure JWT Authentication and Authorization
            var jwtSettings = new JwtSettings();
            builder.Configuration.Bind(nameof(JwtSettings), jwtSettings);
            builder.builder.Services.AddSingleton(jwtSettings);

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
            builder.builder.Services.AddSingleton(tokenValidationParameters);

            builder.builder.Services.AddAuthentication(options =>
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
            builder.builder.Services.AddDbContext<AuthDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("AuthAPI")));

            var identityOptionsConfig = new IdentityOptions();
            builder.Configuration.GetSection(nameof(IdentityOptions)).Bind(identityOptionsConfig);

            builder.builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
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
            builder.builder.Services.AddDataProtection()
                .SetDefaultKeyLifetime(TimeSpan.FromDays(30));
            builder.builder.Services.AddSingleton<IDataEncryption, RouteDataProtection>();

            // Register InMemory Cache services
            builder.Services.AddMemoryCache();
            builder.Services.AddSingleton<ICacheProvider, CacheProvider>();

            // Register Fluent Email Services
            var emailConfig = new EmailConfiguration();
            configuration.GetSection(nameof(EmailConfiguration)).Bind(emailConfig);

            builder.Services.AddFluentEmail(defaultFromEmail: emailConfig.Email)
                .AddRazorRenderer()
                .AddMailKitSender(new SmtpClientOptions()
                {
                    Server = emailConfig.Host,
                    Port = emailConfig.Port,
                    //User = emailConfig.Email,
                    //Password = emailConfig.Password,
                    //RequiresAuthentication = true,
                    PreferredEncoding = "utf-8",
                    UsePickupDirectory = true,
                    MailPickupDirectory = @"C:\Users\mgayle\email",
                    UseSsl = emailConfig.EnableSsl
                });

            // Register Email Notification Service
            builder.Services.AddScoped<IEmailNotification, EmailNotificationService>();

            // Register Names HTTP Client
            builder.Services.AddHttpClient(name: "GitHub", client =>
            {
                client.BaseAddress = new Uri("https://api.github.com/");
                client.DefaultRequestHeaders.Add(name: "Accept", value: "application/vnd.github.v3+json");
                client.DefaultRequestHeaders.Add(name: "User-Agent", value: "HttpClientFactoryExample");
            })
                .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromMilliseconds(300)));

            // Register GitHubApiService
            builder.Services.AddScoped<IGitHubService, GitHubApiService>();

            var app = builder.Build();

            app.Run();
        }
    }
}
