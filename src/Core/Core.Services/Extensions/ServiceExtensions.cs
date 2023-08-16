using Alachisoft.NCache.Caching.Distributed;
using Asp.Versioning;
using Core.Services.Caching;
using Core.Services.Interfaces;
using Core.Services.Options;
using Core.SharedKernel.Config;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Core.Services.Extensions;
public static class ServiceExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JWTTokenConfig>(configuration.GetSection("JwtTokenConfig"))
            .AddSingleton(x => x.GetRequiredService<IOptions<JWTTokenConfig>>().Value);

        services.AddLogging(options =>
        {
            options.AddConsole();
        });

        //services.AddMemoryCache();
        services.AddHttpContextAccessor();

        // register services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenService, TokenService>();

        // if DistributedCaching is enabled
        // return an instance of DistributedCachingService implementation
        // else return an In-Memory caching implementation
        services.AddSingleton<ICachingService>(x =>
        {
            if (configuration.GetValue<bool>("IsDistributedCachingEnabled"))
            {
                return ActivatorUtilities.CreateInstance<DistributedCachingService>(x);
            }
            else
            {
                return ActivatorUtilities.CreateInstance<CachingService>(x);
            }
        });

        services.AddNCacheDistributedCache(configuration.GetSection("NCacheSettings"));

        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
        return services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
                       .AddAutoMapper(Assembly.GetExecutingAssembly())
                       .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));     // MediatR, this will scan and register everything that inherits IRequest

    }

    public static IServiceCollection AddSwaggerWithVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(setup =>
        {
            setup.ApiVersionReader = new Asp.Versioning.UrlSegmentApiVersionReader();
            setup.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
            setup.AssumeDefaultVersionWhenUnspecified = true;
            setup.ReportApiVersions = true;
            setup.Policies.Sunset(0.9)
                        .Effective(DateTimeOffset.Now.AddDays(60))
                        .Link("policy.html")
                        .Title("Versioning Policy")
                        .Type("text/html");
        }).AddApiExplorer(options =>
        {
            // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
            // note: the specified format code will format the version as "'v'major[.minor][-status]"
            options.GroupNameFormat = "'v'VVV";

            // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
            // can also be used to control the format of the API version in route templates
            options.SubstituteApiVersionInUrl = true;
        })
        // this enables binding ApiVersion as a endpoint callback parameter. if you don't use it, then
        // you should remove this configuration
        .EnableApiVersionBinding();

        services.AddVersionedApiExplorer(setup =>
        {
            setup.GroupNameFormat = "'v'VVV";
            setup.SubstituteApiVersionInUrl = true;
        });

        services.ConfigureOptions<ConfigureSwaggerOptions>()
            .AddSwaggerGen(options =>
            {
                // for further customization
                // options.OperationFilter<DefaultValuesFilter>();
            });

        return services;
    }

    public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services)
    {
        services.ConfigureOptions<ConfigureJwtBearerOptions>()
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        return services;
    }
}
