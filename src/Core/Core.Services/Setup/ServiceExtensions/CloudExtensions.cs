using Core.Services.Caching;
using Core.Services.Cloud.AWS;
using Core.Services.Cloud.Azure;
using Core.Services.Cloud.GCP;
using Core.Services.Cloud.IBM;
using Core.Services.Cloud.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Services.Setup.ServiceExtensions;
public static class CloudExtensions
{
    public static IServiceCollection AddSecretsProvider(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddSingleton<ISecretsProvider>(provider =>
        {
            var cloudProvider = configuration.GetSection("CloudProvider").Value;
            var distributedCachingService = provider.GetRequiredService<DistributedCachingService>();

            return cloudProvider switch
            {
                "Azure" => new AzureSecretsProvider(distributedCachingService, configuration),
                "AWS" => new AWSSecretsProvider(distributedCachingService, configuration),
                "GCP" => new GCPSecretsProvider(distributedCachingService, configuration),
                "IBM" => new IBMSecretsProvider(distributedCachingService, configuration),
                _ => throw new NotSupportedException("Cloud provider not supported.")
            };
        });
        return services;
    }
}
