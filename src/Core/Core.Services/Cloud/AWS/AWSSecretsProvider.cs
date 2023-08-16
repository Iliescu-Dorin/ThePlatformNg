using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Core.Services.Caching;
using Core.Services.Cloud.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Core.Services.Cloud.AWS;
public class AWSSecretsProvider : ISecretsProvider
{
    private readonly DistributedCachingService _cachingService;
    private readonly IAmazonSecretsManager _secretsManager;

    public AWSSecretsProvider(DistributedCachingService cachingService, IConfiguration configuration)
    {
        _cachingService = cachingService;
        _secretsManager = new AmazonSecretsManagerClient(
            RegionEndpoint.GetBySystemName(configuration["AWSKeyVault:Region"])
            ?? RegionEndpoint.EUCentral1);
    }

    public async Task<string> GetSecretAsync(string secretName)
    {
        var secretValueResponse = await _secretsManager.GetSecretValueAsync(new GetSecretValueRequest
        {
            SecretId = secretName
        });

        return secretValueResponse.SecretString;
    }

    public async Task<string> GetCachedSecretAsync(string secretName)
    {
        var cachedSecret = await _cachingService.GetItemAsync<string>(secretName);
        if (cachedSecret != null)
        {
            return cachedSecret;
        }

        var secret = await GetSecretAsync(secretName);
        await _cachingService.SetItemAsync(secretName, secret);
        return secret;
    }
}
