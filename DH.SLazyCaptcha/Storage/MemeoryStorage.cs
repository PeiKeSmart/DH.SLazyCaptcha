using DH.SLazyCaptcha.Storage.Caches;

namespace DH.SLazyCaptcha.Storage;

public class MemeoryStorage : IStorage
{
    private readonly MemoryCache Cache;

    public String StoreageKeyPrefix { get; set; } = String.Empty;

    public MemeoryStorage() => Cache = MemoryCache.Default;

    private String WrapKey(String key) => $"{StoreageKeyPrefix}{key}";

    public String Get(String key) => Cache.Get(WrapKey(key));

    public void Remove(String key) => Cache.Remove(WrapKey(key));

    public void Set(String key, String value, DateTimeOffset absoluteExpiration) => Cache.Set(WrapKey(key), value, absoluteExpiration);
}
