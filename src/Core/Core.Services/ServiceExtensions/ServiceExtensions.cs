using Alachisoft.NCache.Caching.Distributed;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Core.Services.Caching;
using Core.Services.Filters;
using Core.Services.Interfaces;
using Core.Services.Jwt;
using Core.Services.Options;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Core.Services.ServiceExtensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSecretsProvider(configuration);

        // Configure HTTP Strict Transport Security Protocol (HSTS)
        services.AddHsts(options =>
        {
            options.Preload = true;
            options.IncludeSubDomains = true;
            options.MaxAge = TimeSpan.FromDays(1);
        });

        // Register and configure CORS
        services.AddCors(options =>
        {
            options.AddPolicy(name: "CorsPolicy",
                options =>
                {
                    options.WithOrigins(configuration.GetSection("Origins").Value)
                    .WithMethods("OPTIONS", "GET", "POST", "PUT", "DELETE")
                    .AllowCredentials();
                });
        });

        services.AddJWTBearerConfiguration(configuration);

        services.AddSwaggerWithVersioning(configuration);

        services.AddLogging(options =>
        {
            options.AddConsole();
        });

        //services.AddMemoryCache();
        services.AddHttpContextAccessor();

        // register services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenService, TokenService>();

        CachingMethods(services, configuration);

        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
        return services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
                       .AddAutoMapper(Assembly.GetExecutingAssembly())
                       .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));     // MediatR, this will scan and register everything that inherits IRequest
    }

    private static void CachingMethods(IServiceCollection services, IConfiguration configuration)
    {
        // if DistributedCaching is enabled
        // return an instance of DistributedCachingService implementation
        // else return an In-Memory caching implementation
        // Method 1
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

        // Instantiate the distributed cache service for Secret Grabbing
        // Method 2
        services.AddDistributedMemoryCache(); // You can replace this with your desired distributed cache provider
        services.AddSingleton<IDistributedCachingService, DistributedCachingService>();
        // Method 3
        services.AddNCacheDistributedCache(configuration.GetSection("NCacheSettings"));
    }

    public static IServiceCollection AddSwaggerWithVersioning(this IServiceCollection services, IConfiguration configuration)
    {
        #region Swagger

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });
        // Swagger OpenAPI Configuration
        var swaggerDocOptions = new SwaggerDocOptions();
        configuration.GetSection(nameof(SwaggerDocOptions)).Bind(swaggerDocOptions);

        // new from different project for Swagger  https://github.com/marlonajgayle/Net6WebApiTemplate/blob/develop/src/Content/src/Net6WebApiTemplate.Api/Program.cs#L230
        services.AddOptions<SwaggerGenOptions>()
                .Configure<IApiVersionDescriptionProvider>((swagger, service) =>
                {
                    foreach (ApiVersionDescription description in service.ApiVersionDescriptions)
                    {
                        swagger.SwaggerDoc(description.GroupName, new OpenApiInfo
                        {
                            Title = swaggerDocOptions.Title,
                            Version = description.ApiVersion.ToString(),
                            Description = swaggerDocOptions.Description,
                            TermsOfService = new Uri("https://github.com/marlonajgayle/Net6WebApiTemplate/blob/develop/LICENSE.md"),
                            Contact = new OpenApiContact
                            {
                                Name = swaggerDocOptions.Organization,
                                Email = swaggerDocOptions.Email
                            },
                            License = new OpenApiLicense
                            {
                                Name = "MIT",
                                Url = new Uri("https://github.com/marlonajgayle/Net6WebApiTemplate")
                            }
                        });
                    }

                    var security = new Dictionary<string, IEnumerable<string>>
                    {
                        {"Bearer", new string[0]}
                    };

                    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme.",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });

                    swagger.OperationFilter<AuthorizeCheckOperationFilter>();

                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    swagger.IncludeXmlComments(xmlPath);
                });

        services.AddSwaggerGen(options =>
        {
            options.OperationFilter<SwaggerDefaultValues>();
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        });

        #endregion Swagger

        // Versioning Method 1
        services.AddApiVersioning(options =>
        {
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
            // reporting api versions will return the headers
            // "api-supported-versions" and "api-deprecated-versions"
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1.0);
            options.Policies.Sunset(0.9)
                            .Effective(DateTimeOffset.Now.AddDays(60))
                            .Link("policy.html")
                            .Title("Versioning Policy")
                            .Type("text/html");
        })
                .AddApiExplorer(options =>
                {
                    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                })
                // this enables binding ApiVersion as a endpoint callback parameter. if you don't use it, then
                // you should remove this configuration.
                .EnableApiVersionBinding();

        // Versioning Method 2
        services.AddApiVersioning(setup =>
        {
            setup.ApiVersionReader = new UrlSegmentApiVersionReader();
            setup.DefaultApiVersion = new ApiVersion(1, 0);
            setup.AssumeDefaultVersionWhenUnspecified = true;
            setup.ReportApiVersions = true;
            setup.Policies.Sunset(0.9)
                        .Effective(DateTimeOffset.Now.AddDays(60))
                        .Link("policy.html")
                        .Title("Versioning Policy")
                        .Type("text/html");
        })
            .AddApiExplorer(options =>
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
