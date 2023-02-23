using Logitar.EventSourcing;
using Logitar.Identity.Actors;

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
  /// Stores an actor into the cache.
  /// </summary>
  /// <param name="actor">The actor to cache.</param>
  void SetActor(Actor actor);
}
