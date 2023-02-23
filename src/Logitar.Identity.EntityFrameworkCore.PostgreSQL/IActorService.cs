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
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The retrieved actor.</returns>
  Task<ActorEntity> GetActorAsync(AggregateId id, CancellationToken cancellationToken = default);

  /// <summary>
  /// Deletes the specified API key actors.
  /// </summary>
  /// <param name="apiKey">The API key actor to delete.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  Task DeleteAsync(ApiKeyEntity apiKey, CancellationToken cancellationToken = default);

  /// <summary>
  /// Updates the specified API key actors.
  /// </summary>
  /// <param name="apiKey">The API key actor to update.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  Task UpdateAsync(ApiKeyEntity apiKey, CancellationToken cancellationToken = default);
}
