using Logitar.EventSourcing;
using Logitar.Identity.Application.Caching;
using Logitar.Identity.Contracts.Actors;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Actors;

internal class ActorService : IActorService
{
  private readonly DbSet<ActorEntity> _actors;
  private readonly ICacheService _cache;

  public ActorService(ICacheService cache, IdentityContext context)
  {
    _actors = context.Actors;
    _cache = cache;
  }

  public async Task<IEnumerable<Actor>> FindAsync(IEnumerable<ActorId> ids, CancellationToken cancellationToken)
  {
    int capacity = ids.Count();
    Dictionary<ActorId, Actor> actors = new(capacity);
    List<string> missingIds = new(capacity);

    foreach (ActorId id in ids)
    {
      if (id != default)
      {
        Actor? actor = _cache.GetActor(id);
        if (actor == null)
        {
          missingIds.Add(id.Value);
        }
        else
        {
          actors[id] = actor;
          _cache.SetActor(actor);
        }
      }
    }

    if (missingIds.Count > 0)
    {
      Mapper mapper = new();

      ActorEntity[] entities = await _actors.AsNoTracking()
        .Where(a => missingIds.Contains(a.Id))
        .ToArrayAsync(cancellationToken);

      foreach (ActorEntity entity in entities)
      {
        Actor actor = mapper.ToActor(entity);
        ActorId id = new(actor.Id);

        actors[id] = actor;
        _cache.SetActor(actor);
      }
    }

    return actors.Values;
  }
}
