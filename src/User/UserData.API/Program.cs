using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Core.Services.APIs;
using Core.Services.Caching;
using Core.Services.Extensions;
using Core.Services.Options;
using Core.SharedKernel.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Globalization;
using System.Reflection;
using UserData.API.Endpoints.V1;
using UserData.API.Filters;
using UserData.Application;
using UserData.Infrastructure.Services;

namespace UserData.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();

        Configure(app, builder.Environment);

        LocalizationRequest(app);

        NWebSecResponseHeaders(app);

        app.Run();
    }

    private static void NWebSecResponseHeaders(WebApplication app)
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

    private static void LocalizationRequest(WebApplication app)
    {
        // List of supported cultures for localization used in RequestLocalizationOptions
        var supportedCultures = new[]
        {
            new CultureInfo("en"),
            new CultureInfo("es")
        };

        // Configure RequestLocalizationOptions with supported culture
        var requestLocalizationOptions = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("en"),

            // Formatting numbers, date etc.
            SupportedCultures = supportedCultures,

            // UI strings that are localized
            SupportedUICultures = supportedCultures
        };
        // Enable Request Localization
        app.UseRequestLocalization(requestLocalizationOptions);
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
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

        // Instantiate the distributed cache service for Secret Grabbing
        services.AddDistributedMemoryCache(); // You can replace this with your desired distributed cache provider
        services.AddSingleton<IDistributedCachingService, DistributedCachingService>();
        services.AddSecretsProvider(configuration);

        services.AddApplication();
        services.AddInfrastructure(configuration);

        // prevents Mvc to throw 400 on invalid RequestBody
        // this is since we're using Fluent to do the same
        // within the Action Method
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        ConfigureJWTBearer(services, configuration);

        // services.AddResponseCaching();

        services.AddControllers(options =>
        {
            options.Filters.Add<AddHandlerHostHeaderResponseFilter>();
        });

        // Add services to the container.
        services.AddProblemDetails();

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

        services.AddAuthorization();

        // Versioning
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

    }

    private static void ConfigureJWTBearer(IServiceCollection services, IConfiguration configuration)
    {
        // configure strongly typed settings object
        var AppSettings = new AppSettings();
        configuration.GetSection("AppSettings").Bind(AppSettings);
        // Now start using it
        string OauthIssuer = AppSettings.OAuthIssuer;
        string OauthClientId = AppSettings.OAuthClientId;
        var OauthSecret = TextEncodings.Base64Url.Decode(AppSettings.OAuthSecret);
        services.AddAuthentication(auth =>
        {
            auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddCookie(cookie =>
        {
            cookie.AccessDeniedPath = "logout";
            cookie.SlidingExpiration = true;
        })
        .AddJwtBearer(jwt =>
        {
            jwt.Audience = OauthClientId;
            jwt.Authority = OauthIssuer;
            jwt.RequireHttpsMetadata = false;
            jwt.SaveToken = true;
            jwt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = OauthIssuer,
                ValidAudience = OauthClientId,
                IssuerSigningKey = new SymmetricSecurityKey(OauthSecret),
                ValidateIssuer = true,
                RequireAudience = true,
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false,
                ValidateTokenReplay = false,
                ValidateActor = false,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero
            };
            jwt.Configuration = new OpenIdConnectConfiguration();

        });
    }

    private static void Configure(WebApplication app, IHostEnvironment env)
    {
        if (env.IsDevelopment() || env.IsEnvironment("Local") || env.IsEnvironment("Test"))
        {
            ConfigureDevelopment(app);
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.

            // Enable HTTP Strict Transport Security Protocol (HSTS)
            app.UseHsts();
        }

        app.UseCors(builder =>
        {
            builder.AllowAnyHeader()
                   .AllowAnyOrigin()
                   .AllowAnyMethod();
        });

        // Enable IP Rate Limiting Middleware
        app.UseIpRateLimiting();



        //Endpoints
        app.MapUserEndpoints();
        app.MapAuthEndpoints();

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks("/health", new HealthCheckOptions()
            {
                ResponseWriter = HealthCheckResponseWriter.WriteHealthCheckResponse,
                AllowCachingResponses = false,

            });
        });

        app.MapControllers();

        //app.UseCaching();

        //app.UseEndpoints(endpoints =>
        //{
        //    endpoints.MapDefaultControllerRoute();
        //});
    }

    private static void ConfigureDevelopment(WebApplication app)
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger(options => { options.RouteTemplate = "api-docs/{documentName}/docs.json"; });
        app.UseSwaggerUI(options =>
        {
            ConfigureSwaggerUI(app, options);
        });
    }

    private static void ConfigureSwaggerUI(WebApplication app, SwaggerUIOptions options)
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
