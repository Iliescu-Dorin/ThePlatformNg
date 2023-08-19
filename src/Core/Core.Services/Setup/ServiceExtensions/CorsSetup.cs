using Microsoft.Extensions.DependencyInjection;

namespace Core.Services.Setup.ServiceExtensions;

/// <summary>
/// Cors startup service
/// </summary>
public static class CorsSetup
{
    public static void AddCorsSetup(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        services.AddCors(c =>
        {
            if (!AppSettings.app(new string[] { "Startup", "Cors", "EnableAllIPs" }).ObjToBool())
            {
                c.AddPolicy(AppSettings.app(new string[] { "Startup", "Cors", "PolicyName" }),
                    policy =>
                    {
                        policy
                            .WithOrigins(AppSettings.app(new string[] { "Startup", "Cors", "IPs" }).Split(','))
                            .AllowAnyHeader() // Ensures that the policy allows any header.
                            .AllowAnyMethod();
                    });
            }
            else
            {
                // Allow any cross-origin request
                c.AddPolicy(AppSettings.app(new string[] { "Startup", "Cors", "PolicyName" }),
                    policy =>
                    {
                        policy
                            .SetIsOriginAllowed((host) => true)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            }
        });
    }
}
