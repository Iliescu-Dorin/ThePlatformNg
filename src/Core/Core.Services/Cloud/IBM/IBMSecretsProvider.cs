using Core.Services.Caching;
using Core.Services.Cloud.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Core.Services.Cloud.IBM;
public class IBMSecretsProvider : ISecretsProvider
{
    private readonly IDistributedCachingService _cachingService;
    private readonly IConfiguration _configuration;


    public IBMSecretsProvider(IDistributedCachingService cachingService, IConfiguration configuration)
    {
        _cachingService = cachingService;
        _configuration = configuration;

    }

    public async Task<string> GetCachedSecretAsync(string cacheKey)
    {
        return (await _cachingService.GetItemAsync<string>(cacheKey)) ?? (await GetSecretAsync(cacheKey));
    }

    public async Task<string> GetSecretAsync(string secretName)
    {
        // If not cached, fetch the secret from IBM Key Protect
        var secret = await FetchSecretFromIBM(secretName);

        // Cache the secret for future use
        await _cachingService.SetItemAsync(secretName, secret);

        return secret;
    }

    private async Task<string> FetchSecretFromIBM(string secretName)
    {
        // Construct the necessary HTTP request to fetch the secret from IBM Key Protect
        var httpClient = new HttpClient();

        // Set headers or authentication tokens required for the IBM Key Protect API
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_configuration["IBMKeyVault:APIKey"]}");

        var requestUri = $"https://ibm-key-protect-url/secret/{secretName}";

        // Send the HTTP request and process the response
        var response = await httpClient.GetAsync(requestUri);
        if (response.IsSuccessStatusCode)
        {
            var secretValue = await response.Content.ReadAsStringAsync();
            return secretValue;
        }
        else
        {
            throw new Exception($"Failed to fetch secret from IBM Key Protect. Status code: {response.StatusCode}");
        }
    }

}
