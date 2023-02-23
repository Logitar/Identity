using Logitar.EventSourcing;
using Logitar.Identity.Actors;
using Logitar.Identity.EntityFrameworkCore.PostgreSQL.Entities;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL;

/// <summary>
/// Exposes methods to help manage actors.
/// </summary>
internal class ActorService : IActorService
{
  /// <summary>
  /// The system actor.
  /// </summary>
  private static readonly Actor _system = new();

  /// <summary>
  /// The cache service.
  /// </summary>
  private readonly ICacheService _cacheService;

  /// <summary>
  /// Initializes a new instance of the <see cref="ActorService"/> using the specified arguments.
  /// </summary>
  /// <param name="cacheService">The cache service.</param>
  public ActorService(ICacheService cacheService)
  {
    _cacheService = cacheService;
  }

  /// <summary>
  /// Retrieves an actor by its identifier.
  /// </summary>
  /// <param name="id">The identifier of the actor.</param>
  /// <returns>The retrieved actor.</returns>
  public ActorEntity GetActor(AggregateId id)
  {
    if (id.Value == _system.Id)
    {
      return new ActorEntity(_system);
    }

    Actor? actor = _cacheService.GetActor(id);
    if (actor == null)
    {
      throw new NotImplementedException();
      // TODO(fpion): add User actors
      // TODO(fpion): add API key actors
    }

    _cacheService.SetActor(actor);

    return new ActorEntity(actor);
  }
}
