using Logitar.EventSourcing;
using Logitar.Identity.Actors;
using Logitar.Identity.ApiKeys;
using Microsoft.Extensions.Caching.Memory;

namespace Logitar.Identity;

/// <summary>
/// Implements methods to store and load objects from a cache.
/// </summary>
internal class CacheService : ICacheService
{
  /// <summary>
  /// The memory cache.
  /// </summary>
  private readonly IMemoryCache _memoryCache;

  /// <summary>
  /// Initializes a new instance of the <see cref="CacheService"/> class using the specified memory cache.
  /// </summary>
  /// <param name="memoryCache">The memory cache.</param>
  public CacheService(IMemoryCache memoryCache)
  {
    _memoryCache = memoryCache;
  }

  /// <summary>
  /// Loads a cached actor by its identifier.
  /// </summary>
  /// <param name="id">The identifier of the actor.</param>
  /// <returns>The cached actor, of null if not found.</returns>
  public Actor? GetActor(AggregateId id) => GetItem<Actor>(GetActorCacheKey(id.Value));

  /// <summary>
  /// Loads a cached API key by its identifier.
  /// </summary>
  /// <param name="id">The identifier of the API key.</param>
  /// <returns>The cached API key, or null if not found.</returns>
  public CachedApiKey? GetApiKey(AggregateId id) => GetItem<CachedApiKey>(GetApiKeyCacheKey(id));

  /// <summary>
  /// Removes a cached API key by its identifier.
  /// </summary>
  /// <param name="id">The identifier of the API key.</param>
  public void RemoveApiKey(AggregateId id)
  {
    RemoveItem(GetActorCacheKey(id.Value));
    RemoveItem(GetApiKeyCacheKey(id));
  }

  /// <summary>
  /// Stores an actor into the cache.
  /// </summary>
  /// <param name="actor">The actor to cache.</param>
  public void SetActor(Actor actor) => SetItem(GetActorCacheKey(actor.Id), actor, TimeSpan.FromHours(1));

  /// <summary>
  /// Stores an API key into the cache.
  /// </summary>
  /// <param name="apiKey">The API key to cache.</param>
  public void SetApiKey(CachedApiKey apiKey) => SetItem(GetApiKeyCacheKey(apiKey.Aggregate.Id), apiKey, TimeSpan.FromHours(1));

  /// <summary>
  /// Loads an object from the cache.
  /// </summary>
  /// <typeparam name="T">The type of the object to load.</typeparam>
  /// <param name="key">The key of the object.</param>
  /// <returns>The cached object, or null if not found.</returns>
  private T? GetItem<T>(object key)
  {
    return _memoryCache.TryGetValue(key, out object? value)
      ? (T?)value
      : default;
  }

  /// <summary>
  /// Removes an object from the cache.
  /// </summary>
  /// <param name="key">The key of the object.</param>
  private void RemoveItem(object key)
  {
    _memoryCache.Remove(key);
  }

  /// <summary>
  /// Stores an object into the cache.
  /// </summary>
  /// <typeparam name="T">The type of the object to store.</typeparam>
  /// <param name="key">The key of the object.</param>
  /// <param name="value">The object to store.</param>
  /// <param name="expiration">The cache expiration of the object.</param>
  private void SetItem<T>(object key, T value, TimeSpan expiration)
  {
    _memoryCache.Set(key, value, expiration);
  }

  /// <summary>
  /// Builds the key used to store and load actors in the cache.
  /// </summary>
  /// <param name="id">The identifier of the actor.</param>
  /// <returns>The cache key.</returns>
  private static string GetActorCacheKey(string id) => $"Actor_{id}";

  /// <summary>
  /// Builds the key used to store and load API keys in the cache.
  /// </summary>
  /// <param name="id">The identifier of the API key.</param>
  /// <returns>The cache key.</returns>
  private static string GetApiKeyCacheKey(AggregateId id) => $"ApiKey_{id}";
}
