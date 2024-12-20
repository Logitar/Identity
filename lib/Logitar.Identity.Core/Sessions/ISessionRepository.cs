using Logitar.Identity.Core.Users;

namespace Logitar.Identity.Core.Sessions;

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
  Task<Session?> LoadAsync(SessionId id, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads a session by the specified unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier.</param>
  /// <param name="version">The version at which to load the session.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The session, if found.</returns>
  Task<Session?> LoadAsync(SessionId id, long? version, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads a session by the specified unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier.</param>
  /// <param name="isDeleted">A value indicating whether or not to load the session if it is deleted.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The session, if found.</returns>
  Task<Session?> LoadAsync(SessionId id, bool? isDeleted, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads a session by the specified unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier.</param>
  /// <param name="version">The version at which to load the session.</param>
  /// <param name="isDeleted">A value indicating whether or not to load the session if it is deleted.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The session, if found.</returns>
  Task<Session?> LoadAsync(SessionId id, long? version, bool? isDeleted, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads the sessions from the event store.
  /// </summary>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found sessions.</returns>
  Task<IReadOnlyCollection<Session>> LoadAsync(CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads the sessions from the event store.
  /// </summary>
  /// <param name="isDeleted">A value indicating whether or not to load deleted sessions.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found sessions.</returns>
  Task<IReadOnlyCollection<Session>> LoadAsync(bool? isDeleted, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads the sessions by the specified list of unique identifiers.
  /// </summary>
  /// <param name="ids">The unique identifiers.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found sessions.</returns>
  Task<IReadOnlyCollection<Session>> LoadAsync(IEnumerable<SessionId> ids, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads the sessions by the specified list of unique identifiers.
  /// </summary>
  /// <param name="ids">The unique identifiers.</param>
  /// <param name="isDeleted">A value indicating whether or not to load deleted sessions.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found sessions.</returns>
  Task<IReadOnlyCollection<Session>> LoadAsync(IEnumerable<SessionId> ids, bool? isDeleted, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads the sessions in the specified tenant.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found sessions.</returns>
  Task<IReadOnlyCollection<Session>> LoadAsync(TenantId? tenantId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads the active sessions of the specified user.
  /// </summary>
  /// <param name="user">The user.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found active sessions.</returns>
  Task<IReadOnlyCollection<Session>> LoadActiveAsync(User user, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads the sessions of the specified user.
  /// </summary>
  /// <param name="user">The user.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found sessions.</returns>
  Task<IReadOnlyCollection<Session>> LoadAsync(User user, CancellationToken cancellationToken = default);

  /// <summary>
  /// Saves the specified session into the store.
  /// </summary>
  /// <param name="session">The session to save.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  Task SaveAsync(Session session, CancellationToken cancellationToken = default);
  /// <summary>
  /// Saves the specified sessions into the store.
  /// </summary>
  /// <param name="sessions">The sessions to save.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  Task SaveAsync(IEnumerable<Session> sessions, CancellationToken cancellationToken = default);
}
