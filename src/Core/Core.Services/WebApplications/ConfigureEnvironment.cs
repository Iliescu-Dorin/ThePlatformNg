using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;

namespace Core.Services.WebApplications;

public static class ConfigureEnvironment
{
    public static void DevelopmentMode(WebApplication app)
    {
        app.UseCors(builder =>
        {
            builder.AllowAnyHeader()
                   .AllowAnyOrigin()
                   .AllowAnyMethod();
        });

        app.UseDeveloperExceptionPage();
        app.UseSwagger(options => { options.RouteTemplate = "api-docs/{documentName}/docs.json"; });
        app.UseSwaggerUI(options =>
        {
            ConfigureSwaggerUI.ConfigureSwaggerUISetup(app, options);
        });
        app.UseAuthentication();
        app.UseAuthorization();
    }

    public static void ProductionMode(WebApplication app)
    {
        app.UseCors(builder =>
        {
            builder.AllowAnyHeader()
                   .AllowAnyOrigin()
                   .AllowAnyMethod();
        });

        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.

        // Enable HTTP Strict Transport Security Protocol (HSTS)
        app.UseHsts();

        // Enable IP Rate Limiting Middleware
        app.UseIpRateLimiting();

        app.UseAuthentication();
        app.UseAuthorization();
    }
}
