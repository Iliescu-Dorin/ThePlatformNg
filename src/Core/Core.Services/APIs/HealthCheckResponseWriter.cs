using Core.SharedKernel.Data.ValueObjects;
using Core.SharedKernel.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace Core.Services.APIs;

public static class HealthCheckResponseWriter
{
    public static async Task WriteHealthCheckResponse(HttpContext httpContext, HealthReport report)
    {
        httpContext.Response.ContentType = "application/json";
        var response = new HealthCheckDTO()
        {
            OverallStatus = report.Status.ToString(),
            TotalDuration = report.TotalDuration.TotalSeconds.ToString("0:0.00"),
            HealthChecks = report.Entries.Select(x =>
                new HealthCheckRecord(
                    x.Value.Status.ToString(),
                    x.Key,
                    x.Value.Description ?? "",
                    x.Value.Duration.TotalSeconds.ToString("0:0.00")
                    )
                )
        };

        await httpContext.Response.WriteAsync(text: JsonConvert.SerializeObject(response, Formatting.Indented));
    }
}
