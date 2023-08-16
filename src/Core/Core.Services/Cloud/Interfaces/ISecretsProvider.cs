namespace Core.Services.Cloud.Interfaces;

public interface ISecretsProvider
{
    Task<string> GetSecretAsync(string secretName);
    Task<string> GetCachedSecretAsync(string secretName);
}
