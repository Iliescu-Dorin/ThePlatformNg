using Core.Services.Caching;
using Core.Services.Cloud.Interfaces;
using Google.Cloud.SecretManager.V1;
using Grpc.Core;
using Microsoft.Extensions.Configuration;

namespace Core.Services.Cloud.GCP;
public class GCPSecretsProvider : ISecretsProvider
{
    private readonly IDistributedCachingService _cachingService;
    private readonly IConfiguration _configuration;
    private readonly SecretManagerServiceClient _secretManagerClient;

    public GCPSecretsProvider(IDistributedCachingService cachingService, IConfiguration configuration)
    {
        _cachingService = cachingService;
        _configuration = configuration;
        _secretManagerClient = SecretManagerServiceClient.Create();
    }

    public async Task<string> GetSecretAsync(string secretName)
    {
        try
        {
            var projectId = _configuration["GCPKeyVault:ProjectId"];
            var secretVersionName = new SecretVersionName(projectId, secretName, "latest");
            var secretVersion = await _secretManagerClient.AccessSecretVersionAsync(secretVersionName);

            return secretVersion.Payload.Data.ToStringUtf8();
        }
        catch (RpcException ex) when (ex.Status.StatusCode == StatusCode.NotFound)
        {
            // Handle secret not found
            return null;
        }
        catch (Exception ex)
        {
            // Handle other exceptions
            return null;
        }
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
