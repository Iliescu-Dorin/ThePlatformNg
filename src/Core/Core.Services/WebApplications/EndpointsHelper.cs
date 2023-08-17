using Core.Services.APIs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Core.Services.WebApplications;

public static class EndpointsHelper
{
    public static void AddHealthCheckEndpoint(WebApplication app)
    {
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks("/health", new HealthCheckOptions()
            {
                ResponseWriter = HealthCheckResponseWriter.WriteHealthCheckResponse,
                AllowCachingResponses = false,
            });
        });
    }
}
