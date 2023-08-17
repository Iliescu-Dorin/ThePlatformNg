using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Core.Services.WebApplications;

public static class ConfigureSwaggerUI
{
    public static void ConfigureSwaggerUISetup(WebApplication app, SwaggerUIOptions options)
    {
        var descriptions = app.DescribeApiVersions();
        options.RoutePrefix = "api-docs";
        // build a swagger endpoint for each discovered API version
        foreach (var description in descriptions)
        {
            var url = $"/api-docs/{description.GroupName}/docs.json";
            var name = description.GroupName.ToUpperInvariant().ToString();
            options.SwaggerEndpoint(url, name);
        }
    }
}
