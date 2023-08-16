using Core.Services.Cloud.Interfaces;
using DreamData.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DreamData.Infrastructure.Services;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, ISecretsProvider secretsProvider)
    {
        //var connectionString = secretsProvider.GetSecretAsync("DreamApi").Result;
        //services.AddDbContext<DreamDbContext>(options =>
        //                options.UseSqlServer(connectionString));

        services.AddDbContext<DreamDbContext>(options =>
                        options.UseSqlServer(configuration.GetConnectionString("DreamApi")));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
};
