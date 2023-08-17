using Core.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Core.Services.Caching;
public class CachingService : ICachingService
{
    private readonly IMemoryCache _memoryCache;

    public CachingService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public T? GetItem<T>(string cacheKey)
    {
        if (_memoryCache.TryGetValue(cacheKey, out T item))
        {
            return item;
        }
        return default;
    }

    public T SetItem<T>(string cacheKey, T item)
    {
        return _memoryCache.Set(cacheKey, item, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24),
            SlidingExpiration = TimeSpan.FromMinutes(60)
        });
    }
}
