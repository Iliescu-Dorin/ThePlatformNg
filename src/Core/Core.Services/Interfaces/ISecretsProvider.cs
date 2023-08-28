namespace Core.Services.Interfaces;

public interface ISecretsProvider
{
    Task<string> GetSecretAsync(string secretName);
    Task<string> GetCachedSecretAsync(string secretName);
}
