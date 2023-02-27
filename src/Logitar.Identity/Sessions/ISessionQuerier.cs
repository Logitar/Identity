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
  /// <summary>
  /// Retrieves a list of user sessions by their aggregate identifier.
  /// </summary>
  /// <param name="ids">The list of aggregate identifiers.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The list of user sessions, or empty if none.</returns>
  Task<IEnumerable<Session>> GetAsync(IEnumerable<AggregateId> ids, CancellationToken cancellationToken = default);
}
