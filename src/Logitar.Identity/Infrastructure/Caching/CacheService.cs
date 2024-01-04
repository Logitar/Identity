using Logitar.EventSourcing;
using Logitar.Identity.Application.Caching;
using Logitar.Identity.Contracts.Actors;
using Microsoft.Extensions.Caching.Memory;

namespace Logitar.Identity.Infrastructure.Caching;

internal class CacheService : ICacheService
{
  private readonly IMemoryCache _memoryCache;

  public CacheService(IMemoryCache memoryCache)
  {
    _memoryCache = memoryCache;
  }

  public Actor? GetActor(ActorId id) => GetItem<Actor>(GetActorKey(id));
  public void SetActor(Actor actor)
  {
    ActorId id = new(actor.Id);
    string key = GetActorKey(id);
    SetItem(key, actor);
  }
  private static string GetActorKey(ActorId id) => $"Actor.Id:{id}";

  private T? GetItem<T>(object key) => _memoryCache.TryGetValue(key, out object? value) ? (T?)value : default;
  private void SetItem<T>(object key, T value) => _memoryCache.Set(key, value);
}
