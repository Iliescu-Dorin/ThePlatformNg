using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Core.Services.Caching;
using Core.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Core.Services.Cloud.Azure;
public class AzureSecretsProvider : ISecretsProvider
{
    private readonly DistributedCachingService _cachingService;
    private readonly IConfiguration _configuration;


    public AzureSecretsProvider(DistributedCachingService cachingService, IConfiguration configuration)
    {
        _cachingService = cachingService;
        _configuration = configuration;
    }

    public async Task<string> GetSecretAsync(string secretName)
    {
        var secretClient = new SecretClient(new Uri(_configuration["AzureKeyVault:VaultUri"]!), new DefaultAzureCredential());
        var secret = await secretClient.GetSecretAsync(secretName);
        return secret.Value.Value;
    }

    public async Task<string> GetCachedSecretAsync(string secretName)
    {
        var cachedSecret = await _cachingService.GetItemAsync<string>(secretName);
        if (cachedSecret != null)
        {
            return cachedSecret;
        }

        var secret = await GetSecretAsync(secretName);
        await _cachingService.SetItemAsync<string>(secretName, secret);
        return secret;
    }
}
