using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserData.Infrastructure.Interfaces;

namespace UserData.Infrastructure.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
                .AddDbContextCheck<UserDbContext>(name: "UserDb Database");
        services.AddDbContext<UserDbContext>(options =>
                        options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
};
