using Core.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace Core.Services.Caching;
public class DistributedCachingService : ICachingService, IDistributedCachingService
{
    private readonly IDistributedCache _cache;

    private readonly DistributedCacheEntryOptions _options = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24),
        SlidingExpiration = TimeSpan.FromMinutes(60)
    };

    public DistributedCachingService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public T? GetItem<T>(string cacheKey)
    {
        var item = _cache.GetString(cacheKey);
        if (item != null)
        {
            return JsonConvert.DeserializeObject<T>(item);
        }
        return default;
    }

    public async Task<T?> GetItemAsync<T>(string cacheKey)
    {
        var item = await _cache.GetStringAsync(cacheKey);
        if (item != null)
        {
            return JsonConvert.DeserializeObject<T>(item);
        }
        return default;
    }

    public T SetItem<T>(string cacheKey, T item)
    {
        _cache.SetString(cacheKey, JsonConvert.SerializeObject(item), _options);
        return item;
    }

    public async Task SetItemAsync<T>(string cacheKey, T item)
    {
        await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(item), _options);
    }
}
