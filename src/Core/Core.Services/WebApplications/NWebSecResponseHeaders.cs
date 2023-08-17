using Microsoft.AspNetCore.Builder;

namespace Core.Services.WebApplications;

public static class NWebSecResponseHeaders
{
    public static void NWebSecResponseHeadersSetup(WebApplication app)
    {
        // Enable NWebSec Security Response Headers
        app.UseXContentTypeOptions();
        app.UseXXssProtection(options => options.EnabledWithBlockMode());
        app.UseXfo(options => options.SameOrigin());
        app.UseReferrerPolicy(options => options.NoReferrerWhenDowngrade());

        // Feature-Policy security header
        app.Use(async (context, next) =>
        {
            context.Response.Headers.Add("Feature-Policy", "geolocation 'none'; midi 'none';");
            await next.Invoke();
        });
    }
}
