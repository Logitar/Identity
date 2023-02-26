using Logitar.EventSourcing;

namespace Logitar.Identity.Sessions;

/// <summary>
/// Exposes methods used to query user session read models.
/// </summary>
public interface ISessionQuerier
{
  /// <summary>
  /// Retrieves an user session by its aggregate identifier.
  /// </summary>
  /// <param name="id">The aggregate identifier.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The user session or null if not found.</returns>
  Task<Session?> GetAsync(AggregateId id, CancellationToken cancellationToken = default);
}
