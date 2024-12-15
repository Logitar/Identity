using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Domain.Sessions;

/// <summary>
/// Defines methods to retrieve and store sessions to an event store.
/// </summary>
public interface ISessionRepository
{
  /// <summary>
  /// Loads a session by the specified unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The session, if found.</returns>
  Task<SessionAggregate?> LoadAsync(SessionId id, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads a session by the specified unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier.</param>
  /// <param name="version">The version at which to load the session.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The session, if found.</returns>
  Task<SessionAggregate?> LoadAsync(SessionId id, long? version, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads a session by the specified unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier.</param>
  /// <param name="includeDeleted">A value indicating whether or not to load the session if it is deleted.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The session, if found.</returns>
  Task<SessionAggregate?> LoadAsync(SessionId id, bool includeDeleted, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads a session by the specified unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier.</param>
  /// <param name="version">The version at which to load the session.</param>
  /// <param name="includeDeleted">A value indicating whether or not to load the session if it is deleted.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The session, if found.</returns>
  Task<SessionAggregate?> LoadAsync(SessionId id, long? version, bool includeDeleted, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads the sessions from the event store.
  /// </summary>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found sessions.</returns>
  Task<IEnumerable<SessionAggregate>> LoadAsync(CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads the sessions from the event store.
  /// </summary>
  /// <param name="includeDeleted">A value indicating whether or not to load deleted sessions.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found sessions.</returns>
  Task<IEnumerable<SessionAggregate>> LoadAsync(bool includeDeleted, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads the sessions by the specified list of unique identifiers.
  /// </summary>
  /// <param name="ids">The unique identifiers.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found sessions.</returns>
  Task<IEnumerable<SessionAggregate>> LoadAsync(IEnumerable<SessionId> ids, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads the sessions by the specified list of unique identifiers.
  /// </summary>
  /// <param name="ids">The unique identifiers.</param>
  /// <param name="includeDeleted">A value indicating whether or not to load deleted sessions.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found sessions.</returns>
  Task<IEnumerable<SessionAggregate>> LoadAsync(IEnumerable<SessionId> ids, bool includeDeleted, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads the sessions in the specified tenant.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found sessions.</returns>
  Task<IEnumerable<SessionAggregate>> LoadAsync(TenantId? tenantId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads the active sessions of the specified user.
  /// </summary>
  /// <param name="user">The user.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found active sessions.</returns>
  Task<IEnumerable<SessionAggregate>> LoadActiveAsync(UserAggregate user, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads the sessions of the specified user.
  /// </summary>
  /// <param name="user">The user.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found sessions.</returns>
  Task<IEnumerable<SessionAggregate>> LoadAsync(UserAggregate user, CancellationToken cancellationToken = default);

  /// <summary>
  /// Saves the specified session into the store.
  /// </summary>
  /// <param name="session">The session to save.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  Task SaveAsync(SessionAggregate session, CancellationToken cancellationToken = default);
  /// <summary>
  /// Saves the specified sessions into the store.
  /// </summary>
  /// <param name="sessions">The sessions to save.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  Task SaveAsync(IEnumerable<SessionAggregate> sessions, CancellationToken cancellationToken = default);
}
