using Core.Services.Filters;
using Core.Services.Setup.ServiceExtensions;
using Core.Services.WebApplications;
using Microsoft.AspNetCore.Mvc;
using UserData.API.Endpoints.V1;
using UserData.Application.Services;
using UserData.Infrastructure.Services;

namespace UserData.API;

public class Program
{
    public static void Main(string[] args)
    {
        // Builder
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder.Services, builder.Configuration);

        // App
        var app = builder.Build();

        if (builder.Environment.IsDevelopment() || builder.Environment.IsEnvironment("Local") || builder.Environment.IsEnvironment("Test"))
        {
            ConfigureEnvironment.DevelopmentMode(app);
            EndpointsConfig(app);
        }
        else
        {
            ConfigureEnvironment.ProductionMode(app);
            EndpointsConfig(app);
        }

        app.UseStaticFiles();

        LocalizationRequest.LocalizationRequestSetup(app);

        NWebSecResponseHeaders.NWebSecResponseHeadersSetup(app);

        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCore(configuration);
        services.AddApplication();
        services.AddInfrastructure(configuration);

        // prevents Mvc to throw 400 on invalid RequestBody
        // this is since we're using Fluent to do the same
        // within the Action Method
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        // services.AddResponseCaching();

        services.AddControllers(options =>
        {
            options.Filters.Add<AddHandlerHostHeaderResponseFilter>();
        });

        // Add services to the container.
        services.AddProblemDetails();

        services.AddAuthorization();
    }

    private static void EndpointsConfig(WebApplication app)
    {
        app.MapUserEndpoints();
        app.MapAuthEndpoints();

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        EndpointsHelper.AddHealthCheckEndpoint(app);

        // app.UseCaching();

        //app.UseEndpoints(endpoints =>
        //{
        //    endpoints.MapDefaultControllerRoute();
        //});

        app.MapControllers();
    }
}
