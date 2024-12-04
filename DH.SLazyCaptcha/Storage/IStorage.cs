namespace DH.SLazyCaptcha.Storage;

public interface IStorage
{
    void Set(String key, String value, DateTimeOffset absoluteExpiration);

    String Get(String key);

    void Remove(String key);
}