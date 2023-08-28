namespace Core.Services.Interfaces;

public interface IDistributedCachingService
{
    Task<T?> GetItemAsync<T>(string cacheKey);
    Task SetItemAsync<T>(string cacheKey, T item);
}
