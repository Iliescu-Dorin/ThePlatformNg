using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace User.Application.Services;

[ExcludeFromCodeCoverage]
public static class RegisterService
{
    public static void ConfigureApplication(this IServiceCollection services, IConfiguration configuration)
    {
        //Here we can register our DI services related to Application

    }
}
