using Microsoft.AspNetCore.SignalR.Redis;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Core.Services.ServiceExtensions;
public static class CacheSetup
{
    /// <summary>
    /// Unified cache registration
    /// </summary>
    /// <param name="services"></param>
    public static void AddCacheSetup(this IServiceCollection services)
    {
        var cacheOptions = App.GetOptions<RedisOptions>();
        if (cacheOptions.Enable)
        {
            // Configure the Redis service startup. This might affect the project's startup speed, but it's reasonable to ensure no runtime errors occur.
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                // Get the connection string
                var configuration = ConfigurationOptions.Parse(cacheOptions.ConnectionString, true);
                configuration.ResolveDns = true;
                return ConnectionMultiplexer.Connect(configuration);
            });
            services.AddSingleton<ConnectionMultiplexer>(p => p.GetService<IConnectionMultiplexer>() as ConnectionMultiplexer);

            // Use Redis
            services.AddStackExchangeRedisCache(options =>
            {
                options.ConnectionMultiplexerFactory = () => Task.FromResult(App.GetService<IConnectionMultiplexer>(false));
                if (!cacheOptions.InstanceName.IsNullOrEmpty()) options.InstanceName = cacheOptions.InstanceName;
            });

            services.AddTransient<IRedisBasketRepository, RedisBasketRepository>();
        }
        else
        {
            // Use in-memory cache
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
        }

        services.AddSingleton<ICaching, Caching>();
    }
}
