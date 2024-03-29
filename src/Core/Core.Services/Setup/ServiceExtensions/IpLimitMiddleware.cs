using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Serilog;

namespace Core.Services.Setup.ServiceExtensions;
public static class IpLimitMiddleware
{
    public static void UseIpLimitMiddle(this IApplicationBuilder app)
    {
        if (app == null) throw new ArgumentNullException(nameof(app));

        try
        {
            if (AppSettings.app("Middleware", "IpRateLimit", "Enabled").ObjToBool())
            {
                app.UseIpRateLimiting();
            }
        }
        catch (Exception e)
        {
            Log.Error($"Error occured limiting ip rate.\n{e.Message}");
            throw;
        }
    }
}
