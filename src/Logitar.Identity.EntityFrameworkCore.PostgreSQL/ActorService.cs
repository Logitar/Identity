using Logitar.EventSourcing;
using Logitar.Identity.Actors;
using Logitar.Identity.EntityFrameworkCore.PostgreSQL.Entities;
using Microsoft.EntityFrameworkCore;

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
  /// The identity context.
  /// </summary>
  private readonly IdentityContext _context;

  /// <summary>
  /// Initializes a new instance of the <see cref="ActorService"/> using the specified arguments.
  /// </summary>
  /// <param name="cacheService">The cache service.</param>
  /// <param name="context">The identity context.</param>
  public ActorService(ICacheService cacheService, IdentityContext context)
  {
    _cacheService = cacheService;
    _context = context;
  }

  /// <summary>
  /// Retrieves an actor by its identifier.
  /// </summary>
  /// <param name="id">The identifier of the actor.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The retrieved actor.</returns>
  public async Task<ActorEntity> GetActorAsync(AggregateId id, CancellationToken cancellationToken)
  {
    if (id.Value == _system.Id)
    {
      return new ActorEntity(_system);
    }

    Actor? actor = _cacheService.GetActor(id);
    if (actor == null)
    {
      // TODO(fpion): add User actors

      ApiKeyEntity? apiKey = await _context.ApiKeys.AsNoTracking()
        .SingleOrDefaultAsync(x => x.AggregateId == id.Value, cancellationToken);
      if (apiKey != null)
      {
        actor = new Actor
        {
          Id = apiKey.AggregateId,
          Type = ActorType.ApiKey,
          DisplayName = apiKey.Title
        };
      }

      if (actor == null)
      {
        throw new InvalidOperationException($"The actor 'Id={id}' could not be found.");
      }
    }

    _cacheService.SetActor(actor);

    return new ActorEntity(actor);
  }

  /// <summary>
  /// Deletes the specified API key actors.
  /// </summary>
  /// <param name="apiKey">The API key actor to delete.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  public async Task DeleteAsync(ApiKeyEntity apiKey, CancellationToken cancellationToken)
  {
    await UpdateAsync(apiKey.AggregateId, new ActorEntity(apiKey, isDeleted: true), cancellationToken);
  }

  /// <summary>
  /// Updates the specified API key actors.
  /// </summary>
  /// <param name="apiKey">The API key actor to update.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  public async Task UpdateAsync(ApiKeyEntity apiKey, CancellationToken cancellationToken)
  {
    await UpdateAsync(apiKey.AggregateId, new ActorEntity(apiKey), cancellationToken);
  }

  /// <summary>
  /// Updates the specified actor in all database entities.
  /// </summary>
  /// <param name="id">The identifier of the actor.</param>
  /// <param name="actor">The actor.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  private async Task UpdateAsync(string id, ActorEntity actor, CancellationToken cancellationToken)
  {
    string serializedActor = actor.Serialize();

    ApiKeyEntity[] apiKeys = await _context.ApiKeys
      .Where(x => x.CreatedById == id || x.UpdatedById == id)
      .ToArrayAsync(cancellationToken);
    foreach (ApiKeyEntity apiKey in apiKeys)
    {
      apiKey.UpdateActors(id, serializedActor);
    }

    RealmEntity[] realms = await _context.Realms
      .Where(x => x.CreatedById == id || x.UpdatedById == id)
      .ToArrayAsync(cancellationToken);
    foreach (RealmEntity realm in realms)
    {
      realm.UpdateActors(id, serializedActor);
    }

    RoleEntity[] roles = await _context.Roles
      .Where(x => x.CreatedById == id || x.UpdatedById == id)
      .ToArrayAsync(cancellationToken);
    foreach (RoleEntity role in roles)
    {
      role.UpdateActors(id, serializedActor);
    }

    await _context.SaveChangesAsync(cancellationToken);
  }
}
