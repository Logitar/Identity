using Logitar.EventSourcing;
using Logitar.Identity.EntityFrameworkCore.PostgreSQL.Entities;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL;

/// <summary>
/// Defines methods to help manage actors.
/// </summary>
internal interface IActorService
{
  /// <summary>
  /// Retrieves an actor by its identifier.
  /// </summary>
  /// <param name="id">The identifier of the actor.</param>
  /// <returns>The retrieved actor.</returns>
  ActorEntity GetActor(AggregateId id);
}
