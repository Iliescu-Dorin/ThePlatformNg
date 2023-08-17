using Core.Services.Behavior;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;

namespace UserData.Application.Services;
// This class implements a rather crude modular configuration of subcomponents, without any ceremony or meta-structure.
// Proper abstractions can be added later if modularization would seem to benefit from them.

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register Application Health Checks
        services.AddHealthChecks()
            .AddCheck<ApplicationHealthCheck>(name: "ObscuraDreamlog API");

        // Register MediatR pipeline behaviors, in the same order the behaviors should be called.

        //services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuditBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheRemovingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

        // custom services
        services.AddHttpContextAccessor();
        services.AddScoped<IAuthService, AuthManager>();
        services.AddTransient<ITokenHelper, JwtHeader>();
        services.AddSingleton<IMailService, MailkitMailService>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<ICurrentUser, CurrentUser>();

        // add httpclient services
        services.AddHttpClient();

        return services;
    }
};
