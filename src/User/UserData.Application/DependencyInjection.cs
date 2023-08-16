using Core.SharedKernel.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace UserData.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register Application Health Checks
        services.AddHealthChecks()
            .AddCheck<ApplicationHealthCheck>(name: "ObscuraDreamlog API");

        // Register MediatR pipeline behaviors, in the same order the behaviors should be called.

        //services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
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
