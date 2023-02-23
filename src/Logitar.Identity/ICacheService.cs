using Logitar.EventSourcing;
using Logitar.Identity.Actors;
using Logitar.Identity.ApiKeys;

namespace Logitar.Identity;

/// <summary>
/// Exposes methods to store and load objects from a cache.
/// </summary>
public interface ICacheService
{
  /// <summary>
  /// Loads a cached actor by its identifier.
  /// </summary>
  /// <param name="id">The identifier of the actor.</param>
  /// <returns>The cached actor, of null if not found.</returns>
  Actor? GetActor(AggregateId id);

  /// <summary>
  /// Loads a cached API key by its identifier.
  /// </summary>
  /// <param name="id">The identifier of the API key.</param>
  /// <returns>The cached API key, or null if not found.</returns>
  CachedApiKey? GetApiKey(AggregateId id);

  /// <summary>
  /// Stores an actor into the cache.
  /// </summary>
  /// <param name="actor">The actor to cache.</param>
  void SetActor(Actor actor);

  /// <summary>
  /// Stores an API key into the cache.
  /// </summary>
  /// <param name="apiKey">The API key to cache.</param>
  void SetApiKey(CachedApiKey apiKey);
}
